import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class TileTypeList extends Component {
  static displayName = TileTypeList.name;

  constructor(props) {
    super(props);
  }

  renderTileTypes(tileTypes) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Color</th>
            <th>&nbsp;</th>
          </tr>
        </thead>
        <tbody>
          {tileTypes.map(tileType =>
            <tr key={tileType.id}>
              <td>{tileType.name}</td>
              <td>{tileType.color}</td>
              <td><input onChange={() => this.props.selectTile(tileType.id)} type="radio" id={tileType.id} name="selectedid" checked={this.props.lastTileId == tileType.id}  value={tileType.id}></input></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents =  this.renderTileTypes(this.props.tileTypes);
    return (
      <div>
        <p>Select from these tile types</p>
        {contents}
      </div>
    );
  }
}