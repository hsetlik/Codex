import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { MetricGraph, MetricGraphQuery } from "../models/dailyData";
import { LanguageProfileDto } from "../models/dtos";


export default class DailyDataStore {
    selectedProfile: LanguageProfileDto | null = null;
    graphLoaded = false;
    currentGraph: MetricGraph | null = null;
    constructor(){
        makeAutoObservable(this);
    }

    loadMetricGraph = async (query: MetricGraphQuery) => {
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
}