import { Component, OnInit, Input } from '@angular/core';
import { MarsWeather } from '../mars-weather';

@Component({
  selector: 'app-sol-number',
  templateUrl: './sol-number.component.html',
  styleUrls: ['./sol-number.component.styl']
})
export class SolNumberComponent implements OnInit {

  @Input() weather!: MarsWeather;
  constructor() { }

  ngOnInit(): void {
  }

}
