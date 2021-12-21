import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import MetricGraphPanel from "./MetricGraphPanel";
import MetricTypeMenu from "./MetricTypeMenu";
import NumDaysDropdown from "./NumDaysDropdown";

export default observer(function MetricView() {
    const {dailyDataStore} = useStore();
    const {currentMetricName, currentNumDays} = dailyDataStore;
    return (
            <Container>
                <Segment>
                    <MetricTypeMenu />
                    <NumDaysDropdown />
                </Segment>
                <MetricGraphPanel metricName={currentMetricName} days={currentNumDays} />
            </Container>
    )
})