import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class UserDevices extends Component {
    static displayName = UserDevices.name;

    constructor(props) {
        super(props);
        this.state = { devices: [], loading: true };
    }

    componentDidMount() {
        this.populateDevicesData();
    }

    static renderDevicesTable(devices) {
        return (
            <table className='table table-striped' aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                    {devices.map(device =>
                        <tr key={device.id}>
                            <td>{device.id}</td>
                            <td>{device.name}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : UserDevices.renderDevicesTable(this.state.devices);

        return (
            <div>
                <h1 id="tableLabel">Your devices</h1>
                {contents}
            </div>
        );
    }

    async populateDevicesData() {
        const token = await authService.getAccessToken();
        const userId = await authService.getUser();
        const response = await fetch(`device/${userId.sub}`, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();
        this.setState({ devices: data, loading: false });
    }
}