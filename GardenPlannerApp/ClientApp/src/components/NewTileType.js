import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class NewTileType extends Component {
  static displayName = NewTileType.name;

  constructor(props) {
    super(props);

    this.state = {
      name: "",
      color: "#ff0000",
      public: true,
      submitting: false
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);

    this.validators = {
      color: x => true,
      name: x => x.length <= 15
    }
  }

  handleChange(event) {
    const { target } = event;
    if (this.validators[target.name](target.value)) {
      let change = {};
      change[target.name] = target.value;
      this.setState(change);
    }
  }

  async handleSubmit(event) {
    event.preventDefault();
    this.setState({
      submitting: true
    });
    await this.submitNewTileType();

    this.setState({
      submitting: false
    });
  }


  async submitNewTileType() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/tiletypes', {
      method: "POST",
      body: JSON.stringify({
        Name: this.state.name,
        Color: this.state.color,
        Public: this.state.public
      }),
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    this.setState({ loading: false });
    this.props.tileCreated(data.id)

  }

  render() {
    return (
      <div>
        <p>New tile type</p>{this.state.submitting ? <p>Creating tile type</p> : (
          <form onSubmit={this.handleSubmit} >
            <div className="form-group">
              <input onChange={this.handleChange} type="text" required className="form-control" id="garden-name" placeholder="Tile type name" name="name" value={this.state.name}></input>
            </div>
            <div className="form-group">
              <input onChange={this.handleChange} type="text" required type="color" className="form-control" id="tiletype-color" name="color" value={this.state.color}></input>
            </div>

            <button type="submit" className="btn btn-primary">Create tile type</button>
          </form>)}
      </div>
    );
  }
}
