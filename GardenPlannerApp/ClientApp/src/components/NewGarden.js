import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class NewGarden extends Component {
    static displayName = NewGarden.name;

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            width: 10,
            height: 10,
            submitting: false
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        
        this.validators = {
            width: x => !isNaN(parseInt(x)),
            height: x => !isNaN(parseInt(x)),
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
            submitting:true
        });
        await this.submitNewGarden();

        this.setState({
            submitting: false
        });
    }


    async submitNewGarden() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/gardens', {
            method: "POST",
            body: JSON.stringify({
                        Name: this.state.name,
                        Width: this.state.width,
                        Height: this.state.height
            }),
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });

    }

    render() {
        return (
            <div>
                <h1>Create new garden</h1>{this.state.submitting ? <p>Creating graden</p> : (
                    <form onSubmit={this.handleSubmit} >
                        <div className="form-group">
                            <label htmlFor="garden-name">Garden name</label>
                            <input onChange={this.handleChange} type="text" required className="form-control" id="garden-name" placeholder="Garden name" name="name" value={this.state.name}></input>
                        </div>
                        <div className="form-group">
                            <label htmlFor="garden-width">Width</label>
                            <input onChange={this.handleChange} type="text" required className="form-control" id="garden-width" name="width" value={this.state.width}></input>
                        </div>

                        <div className="form-group">
                            <label htmlFor="garden-height">Height</label>
                            <input onChange={this.handleChange} type="text" required className="form-control" id="garden-height" name="height" value={this.state.height}></input>
                        </div>
                        <button type="submit" className="btn btn-primary">Create garden</button>
                    </form>)}
            </div>
        );
    }
}
