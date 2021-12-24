import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { CartesianGrid, Line, LineChart, XAxis, YAxis } from "recharts";
import { Container } from "semantic-ui-react";
import { getGraphDataPoints, getGraphQuery } from "../../app/models/dailyData";
import { useStore } from "../../app/stores/store";

interface Props {
    metricName: string,
    days: number,
    profileId: string
}

export default observer(function MetricGraphPanel({metricName, days, profileId}: Props) {
    const {dailyDataStore} = useStore();
    const {currentGraph, graphLoaded, loadMetricGraph} = dailyDataStore;
    const query = getGraphQuery(metricName, days, profileId!);
    useEffect(() => {
        if (!graphLoaded){
            loadMetricGraph(query);
            console.log(`Loaded Graph`);
        }
    }, [query, loadMetricGraph, graphLoaded]);
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
            <LineChart data={data}>
                <Line type="monotone" dataKey="uv" stroke="#8884d8" />
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="date" />
                <YAxis />
            </LineChart>
        </Container>
    )
})