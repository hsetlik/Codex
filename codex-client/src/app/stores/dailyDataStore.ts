import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { allMetricNames, MetricGraph, MetricGraphQuery } from "../models/dailyData";


export default class DailyDataStore {
    graphLoaded = false;
    currentGraph: MetricGraph | null = null;
    currentMetricName = allMetricNames[0];
    currentNumDays = 7;
    constructor(){
        makeAutoObservable(this);
    }

    loadMetricGraph = async (query: MetricGraphQuery) => {
        console.log(query);
        this.graphLoaded = false;
        try {
            const newGraph = await agent.Profile.getMetricGraph(query);
            runInAction(() => {
                this.currentGraph = newGraph;
                this.graphLoaded = true;
            }) 
        } catch (error) {
            console.log(error);
        }
    }

    setCurrentMetricName = (name: string) => {
        this.currentMetricName = name;
    }

    setCurrentNumDays = (value: number) => {
        this.currentNumDays = value;
    }
}