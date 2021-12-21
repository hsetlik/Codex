
export const getDotnetDateTime = (date: Date) : string => {
    const day = date.getDate() + 1;
    const month = date.getMonth() + 1;
    let dayStr = (day > 9) ? date.getDate().toString() : `0${date.getDate().toString()}`;
    let monthStr = (month > 9) ? date.getMonth().toString() : `0${date.getMonth().toString()}`;
    return `${date.getFullYear()}-${monthStr}-${dayStr}`;
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

//const getDateString = ()

export const getGraphQuery = (name: string, days: number, profileId: string) : MetricGraphQuery => {
    const endDate = new Date(Date.now());
    console.log(`End date is: ${endDate}`);
    const startDate = new Date(Date.now() - days);
    console.log(`Start date is: ${startDate}`);
    return {
        metricName: name,
        languageProfileId: profileId,
        start: getDotnetDateTime(startDate),
        end: getDotnetDateTime(endDate)
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