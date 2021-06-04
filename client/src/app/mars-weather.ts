export interface MarsWeather {
    sol: number;
    firstUTC: Date;
    lastUTC: Date;
    season: Season;
    atmosphericPressure: DataDescription;
    photos: string[];
    rovers: RoverInfo[];
}

export enum Season {
    winter,
    spring,
    summer,
    autumn
}

export interface DataDescription {
    average: number;
    totalCount: number;
    minimum: number;
    maximum: number;
}

export interface RoverInfo {
    name: string;
    landingDate: Date;
    launchDate: Date;
    status: string;
}