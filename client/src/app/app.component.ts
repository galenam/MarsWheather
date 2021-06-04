import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { GraphqlService } from './graphql.service';
import { MarsWeather } from './mars-weather';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.styl']
})
export class AppComponent {
  title = 'client';
  private solsObservable: Observable<Subscription>;
  private solSubscription: Subscription;
  public weather: Array<MarsWeather> = Array(0);
  public weatherToShow: MarsWeather | null = null;

  constructor(private graphql: GraphqlService) {
    this.solsObservable = this.graphql.getSols();
    this.solSubscription = this.solsObservable.subscribe((result: any) => {

      this.weather = result.data.weather;
    });
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.solSubscription.unsubscribe();
  }

  onShow(item: MarsWeather) {
    this.weatherToShow = item;
    console.log(item);
  }
}
