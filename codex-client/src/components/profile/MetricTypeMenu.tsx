import { observer } from "mobx-react-lite";
import React from "react";
import { Menu } from "semantic-ui-react";
import { allMetricNames } from "../../app/models/dailyData";
import { useStore } from "../../app/stores/store";


export default observer(function MetricTypeMenu() {
    const {dailyDataStore} = useStore();
    const {currentMetricName, setCurrentMetricName} = dailyDataStore;
    return (
        <Menu vertical={false} attached="top" >
            {allMetricNames.map(name => {
                return (
                    <Menu.Item key={name}
                    active={currentMetricName === name}
                    onClick={() => setCurrentMetricName(name)}
                    >
                        {name}
                    </Menu.Item>
                )
            })}
        </Menu>
    )
})