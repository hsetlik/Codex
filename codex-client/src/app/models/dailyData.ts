
export const getDotnetDateTime = (date: Date) : string => {
    let dayStr = (date.getDate() < 10) ? date.getDate().toString() : `0${date.getDate().toString()}`;
    let monthStr = (date.getMonth() < 10) ? date.getMonth().toString() : `0${date.getMonth().toString()}`;
    return `${date.getFullYear()}-${dayStr}-${monthStr}`;
}

export interface DailyDataPoint {
    metricName: string,
    valueTypeName: string,
    valueString: string,
    languageProfileId: string,
    dateTime: string
}

export interface MetricGraphQuery {
    metricName: string,
    languageProfileId: string,
    start: string,
    end: string
}

export interface MetricGraph {
    metricName: string,
    languageProfileId: string,
    start: string,
    end: string,
    dataPoints: DailyDataPoint[]
}

export const getGraphQuery = (name: string, days: number, profileId: string) : MetricGraphQuery => {
    const endDate = new Date(Date.now());
    const startDate = new Date(Date.now() - days);
    return {
        metricName: name,
        languageProfileId: profileId,
        start: startDate.toDateString(),
        end: endDate.toDateString()
    }
}

export interface GraphDataPoint {
    date: string,
    uv: number
}

export const getGraphDataPoints = (graph: MetricGraph) : GraphDataPoint[] => {
    let dataPoints: GraphDataPoint[] = [];
    for (let p of graph.dataPoints) {
        dataPoints.push({
            date: p.dateTime,
            uv: parseInt(p.valueString)
        });
    }
    return dataPoints;
}

export const allMetricNames = [
    "KnownWords",
    "NumUserTerms"
]