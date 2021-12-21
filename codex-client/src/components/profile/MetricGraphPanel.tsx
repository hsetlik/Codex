import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { CartesianGrid, Line, LineChart, XAxis, YAxis } from "recharts";
import { Segment } from "semantic-ui-react";
import { getGraphDataPoints, getGraphQuery } from "../../app/models/dailyData";
import { useStore } from "../../app/stores/store";

interface Props {
    metricName: string,
    days: number
}

export default observer(function MetricGraphPanel({metricName, days}: Props) {
    const {dailyDataStore, userStore: {selectedProfile}} = useStore();
    const {currentGraph, graphLoaded, loadMetricGraph} = dailyDataStore;
    const profileId = selectedProfile?.languageProfileId;
    useEffect(() => {
        const query = getGraphQuery(metricName, days, profileId!);
        loadMetricGraph(query);
    }, [metricName, days, profileId, loadMetricGraph]);
    if (!graphLoaded || currentGraph == null) {
        return (
            <div></div>
        )
    }
    const data = getGraphDataPoints(currentGraph)
    return (
        <Segment>
            <LineChart data={data}>
                <Line type="monotone" dataKey="uv" stroke="#8884d8" />
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="date" />
                <YAxis />
            </LineChart>
        </Segment>
    )
})