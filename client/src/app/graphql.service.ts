import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { MarsWeather } from './mars-weather';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {

  constructor(private apollo: Apollo) { }

  getSols() {
    this.apollo.watchQuery({
      query: gql`
      	{
    weather {
      sol
    }
  }`
    })
      .valueChanges.subscribe((result: any) => {
        console.log(result);
      })
  }

}
