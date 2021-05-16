import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { MarsWheather } from './mars-wheather';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {

  constructor(private apollo: Apollo) { }

  getSols() {
    this.apollo.watchQuery({
      query: gql`
      	{
    wheather {
      sol
    }
  }`
    })
      .valueChanges.subscribe((result: any) => {
        console.log(result);
      })
  }

}
