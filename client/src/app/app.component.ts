import { Component } from '@angular/core';
import { GraphqlService } from './graphql.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.styl']
})
export class AppComponent {
  title = 'client';

  constructor(private graphql: GraphqlService) { }

  ngOnInit() {
    this.graphql.getSols();
  }
}
