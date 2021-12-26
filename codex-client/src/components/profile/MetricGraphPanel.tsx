import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { CartesianGrid, Line, LineChart, XAxis, YAxis } from "recharts";
import { Container } from "semantic-ui-react";
import { getDotnetDateTime, getGraphDataPoints, getGraphQuery } from "../../app/models/dailyData";
import { useStore } from "../../app/stores/store";

interface Props {
    metricName: string,
    days: number,
    profileId: string
}

export default observer(function MetricGraphPanel({metricName, days, profileId}: Props) {
    console.log(`range is ${days} days`);
    const {dailyDataStore} = useStore();
    const {currentGraph, graphLoaded, loadMetricGraph} = dailyDataStore;
    const query = getGraphQuery(metricName, days, profileId!);
    let currentStart = getDotnetDateTime(new Date(currentGraph?.start!));
    let currentEnd = getDotnetDateTime(new Date(currentGraph?.end!));
    console.log(`query start string is: ${query.start}`);
    console.log(`query end string is: ${query.end}`);
    console.log(`current start string is: ${currentStart}`);
    console.log(`current end string is: ${currentEnd}`);

    useEffect(() => {
        if (!graphLoaded || currentGraph?.metricName !== metricName || query.start !== currentStart){
            loadMetricGraph(query);
            console.log(`Loaded Graph`);
        }
    }, [query, loadMetricGraph, graphLoaded, currentGraph, metricName, currentStart]);
    if (!graphLoaded || currentGraph == null) {
        return (
            <div></div>
        )
    }
    const data = getGraphDataPoints(currentGraph);
    for(let d of data) {
        console.log(`Datapoint for ${d.date} has value ${d.uv} and index ${d.idx}`);
    }
    return (
        <Container>
            <LineChart data={data} 
            width={window.screen.availWidth * 0.45}
            height={window.screen.availHeight * 0.45}>
                <Line type="monotone" dataKey="uv" stroke="#8884d8" />
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="date" />
                <YAxis dataKey="uv"/>
            </LineChart>
        </Container>
    )
})