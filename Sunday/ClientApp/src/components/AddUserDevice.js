import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class AddUserDevice extends Component {
    static displayName = AddUserDevice.name;

    constructor(props) {
        super(props);
        this.state = { deviceName: '' };

        this.handleChange = this.handleChange.bind(this);
        this.createDevice = this.createDevice.bind(this);
    }

    componentDidMount() {
    }

    handleChange(event) {
        this.setState({ deviceName: event.target.value });
    }

    renderDeviceForm() {
        return (
            <form onSubmit={this.createDevice}>
                <div class= "form-group">
                    <label class="form-control">Name</label>
                    <input type="text" class="form-control" onChange={this.handleChange} />
                </div>
                <div class="form-group">
                    <input type="submit" value="Create" class="btn btn-primary" />
                </div>
            </form>
        );
    }

    render() {
        let contents = this.renderDeviceForm();

        return (
            <div>
                { contents }
            </div>
        );
    }

    async createDevice(event) {
        event.preventDefault();

        const token = await authService.getAccessToken();
        const user = await authService.getUser();
        console.log(user);

        const device = {
            "ApplicationUserID": user.sub,
            "Name": this.state.deviceName
        };

        const response = await fetch("device", {
            headers: !token ? {} : {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(device),
            method: 'POST',
        });

        console.log(response);
    }

    //async populateDevicesData() {

    //    const response = await fetch(`device/${userId.sid}`, {
    //        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    //    });

    //    const data = await response.json();
    //    this.setState({ devices: data, loading: false });
    //}

}