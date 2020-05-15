import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import { TileTypeList } from './TileTypeList';
import { NewTileType } from './NewTileType';

export class Garden extends Component {
    static displayName = Garden.name;

  constructor(props) {
    super(props);
    this.state = { garden: null, tileTypes: [], loading: true, lastTileId:""};
    this.tileCreated = this.tileCreated.bind(this);
    this.selectTile = this.selectTile.bind(this);
    this.setTile = this.setTile.bind(this);
    this.shareUnshare = this.shareUnshare.bind(this);
  }

  componentDidMount() {
    this.loadGarden();
    this.populateTileTypes();
  }


  async tileCreated(id) {
    if (this.state.garden.readOnly) {
      return;
    }
    await this.populateTileTypes();
    this.setState({
      lastTileId:id
    });
  }

  selectTile(id) {
    if (this.state.garden.readOnly) {
      return;
    }
    this.setState({
      lastTileId: id
    });
  }

  async setTile(x, y) {
    if (this.state.garden.readOnly) {
      return;
    }
    const token = await authService.getAccessToken();

    let tile = this.state.garden.tiles.find(t => t.x === x && t.y === y);
    if (tile) {
      await fetch('api/gardentiles/' + tile.id, {
        method: "DELETE",
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      this.setState({
        garden: {
          ...this.state.garden,
          tiles: this.state.garden.tiles.filter(x => x.id != tile.id)
        }
      });
    }

    const response = await fetch('api/gardentiles', {
      method: "POST",
      body: JSON.stringify({
        x: x,
        y: y,
        gardenId: this.state.garden.id,
        tileTypeId: this.state.lastTileId
      }),
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    this.setState({
      garden: {
        ...this.state.garden,
        tiles: [...this.state.garden.tiles, data]
      }
    });
  }

  async shareUnshare() {
    const token = await authService.getAccessToken();

    const response = await fetch('api/gardens/share', {
      method: "POST",
      body: JSON.stringify({
        id: this.state.garden.id,
        public: !this.state.garden.public
      }),
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    this.setState({
      garden: data
    });
  }


  renderGarden(garden, tileTypes, lastTileId) {
    function Tile({ tileType, x, y, setTile}) {
      return (
        <td style={{
          backgroundColor: tileType.color,
          width: 50,
          border: "1px solid black"
        }} onClick={() => setTile(x,y)} >
          {tileType.name}
        </td>
      )
    }

    function TileRow({ row, setTile}) {
      return (<tr style={{
        height: 50,
        border: "1px solid black"
      }}>
        {row.map(tile => <Tile key={`${tile.x}_${tile.y}`} {...tile} setTile={setTile}> </Tile>)}
      </tr>);
    };

    function dummyTile(x, y) {
      return {
        x: x,
        y: y,
        tileType: {
          name: "Ground",
          color: "brown"
        }
      }
    };
    let allTiles = [];
    for (let i = 0; i < garden.width; i++) {
      allTiles.push([])
      for (let j = 0; j < garden.height; j++) {
        let tile = garden.tiles.find(t => t.x === i && t.y === j);
        if (tile) {
          allTiles[i].push(tile);
        } else {
          allTiles[i].push(dummyTile(i, j));
        }
      }
    }

    return (
      <div className="row">
        
        <div className="col-sm-8">
          <div className="row">
            <div className="col-sm-6">
              Garden: {this.state.garden.name}
            </div>
           
            <div className="col-sm-6">
              {!garden.readOnly ? (
                <button className="btn btn-primary" onClick={this.shareUnshare}>{garden.public ? "Unshare" : "Share"}</button>
              ) : ("Garden of: " + garden.owner.userName)}
            </div>
          </div>
          <div className="row">
            <table
              style={{
                borderCollapse: "collapse",
                border: "1px solid black"
              }}>
              <tbody>
                {allTiles.map((row, index) => <TileRow key={index} row={row} setTile={this.setTile}></TileRow>)}
              </tbody>
              </table>
          </div>
        </div>
        <div className="col-sm-4">
          {!this.state.garden.readOnly ?
            (<>
              <TileTypeList lastTileId={lastTileId} selectTile={this.selectTile} tileTypes={tileTypes} ></TileTypeList>
              <NewTileType tileCreated={this.tileCreated}></NewTileType>
            </>):
            (<></>)}
        </div>
      </div>
    )
  }

  render() {
    let contents = this.state.loading || !this.state.garden 
      ? <p><em>Loading...</em></p>
      : this.renderGarden(this.state.garden, this.state.tileTypes, this.state.lastTileId);

    return (
      <div>
        {contents}
      </div>
    );
  }
  async loadGarden() {
    const { gardenId } = this.props.match.params
    const token = await authService.getAccessToken();
    const response = await fetch('api/gardens/' + gardenId, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ garden: data, loading: false });
  }




  async populateTileTypes() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/tiletypes', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    if (this.state.lastTileId === "") {
      this.state.lastTileId = data?.[0]?.id
    }
    this.setState({ tileTypes: data, loading: false, lastTileId: this.state.lastTileId });
  }
}
