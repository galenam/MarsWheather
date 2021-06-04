import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { Observable } from 'rxjs';
import { Subscription } from 'rxjs';
import { MarsWeather } from './mars-weather';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {

  constructor(private apollo: Apollo) { }

  getSols(): Observable<any> {
    return this.apollo.watchQuery({
      query: gql`
      	{
    weather {
      sol,
      firstUTC,
      lastUTC,
      season,
      atmosphericPressure{
        average,
        maximum,
        minimum,
        totalCount
      },
      photos,
      rovers {
        name,
        launchDate,
        landingDate,
        status
      }
    }
  }`
    })
      .valueChanges
  }
}
