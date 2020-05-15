import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class GardenList extends Component {
    static displayName = GardenList.name;

    constructor(props) {
        super(props);
        this.state = { gardens: [], loading: true };
    }

    componentDidMount() {
        this.populateGardens();
    }

    openGarden(garden) {

        window.location.replace("garden/" + garden.id);
    }

    renderGardenTable(gardens) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Width</th>
                        <th>Height</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {gardens.map(garden =>
                        <tr key={garden.id}>
                            <td>{garden.name}</td>
                            <td>{garden.width}</td>
                            <td>{garden.height}</td>
                            <td><a onClick={() => this.openGarden(garden)}>Open</a></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGardenTable(this.state.gardens);

        return (
            <div>
                <h1 id="tabelLabel" >Your gardens</h1>
                <p>This is the list of all gardens you have access to</p>
                {contents}
            </div>
        );
    }

    async populateGardens() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/gardens', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ gardens: data, loading: false });
    }
}