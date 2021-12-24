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
    const {profileStore} = useStore();
    const {currentMetricName, currentNumDays} = profileStore;
    console.log(`MetricView has profile id: ${profileId}`);
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