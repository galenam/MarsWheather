import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-mars-photo',
  templateUrl: './mars-photo.component.html',
  styleUrls: ['./mars-photo.component.styl']
})
export class MarsPhotoComponent implements OnInit {
  @Input() photos: Array<string> = new Array();
  constructor() { }

  ngOnInit(): void {
  }

}
