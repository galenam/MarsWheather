import { Component, Input, OnInit } from '@angular/core';
import { MarsWeather } from '../mars-weather';

@Component({
  selector: 'app-weather-data',
  templateUrl: './weather-data.component.html',
  styleUrls: ['./weather-data.component.styl']
})
export class WeatherDataComponent implements OnInit {
  @Input() weather: MarsWeather | null = null;
  constructor() {
  }

  ngOnInit(): void {
    console.log(this.weather);
  }

}