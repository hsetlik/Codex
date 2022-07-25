import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import MetricGraphPanel from "./MetricGraphPanel";
import MetricTypeMenu from "./MetricTypeMenu";
import NumDaysDropdown from "./NumDaysDropdown";

interface Props {
    profileId: string
}

export default observer(function MetricView({profileId}: Props) {
    const {dailyDataStore} = useStore();
    const {currentMetricName, currentNumDays} = dailyDataStore;
    return (
            <Container>
                <Segment>
                    <MetricTypeMenu />
                    <NumDaysDropdown />
                </Segment>
                <MetricGraphPanel profileId={profileId} metricName={currentMetricName} days={currentNumDays} />
            </Container>
    )
})