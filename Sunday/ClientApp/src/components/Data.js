import React, { Component } from 'react';
import Chart from "react-apexcharts";
import authService from './api-authorization/AuthorizeService'

export class DeviceData extends Component {
    static displayName = DeviceData.name;

    constructor(props) {
        super(props);
        this.state = { deviceData: [], loading: true };
    }

    componentDidMount() {
        const { match: { params } } = this.props;
        this.populateDevicesData(params.deviceId);
    }

    static renderDataTable(deviceData) {
        var chartData = [{
            data: deviceData.map(d => {
                return {
                    x: d.timestamp,
                    y: d.value
                }
            })
        }]

        var options = {
            xaxis: {
                type: 'datetime'
            }
        }

        return (
            <Chart series={chartData} options={options} type="line" />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : DeviceData.renderDataTable(this.state.deviceData);

        return (
            <div>
                <h1 id="tableLabel">Device data</h1>
                {contents}
            </div>
        );
    }

    async populateDevicesData(deviceId) {
        const token = await authService.getAccessToken();
        const response = await fetch(`device/${deviceId}?startTime=2021-01-27T00:00&endTime=2021-03-30T23:59`, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        var data = await response.json();
        if (data == null) {
            data = [];
        }
        this.setState({ deviceData: data, loading: false });
    }
}