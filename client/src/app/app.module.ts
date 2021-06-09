import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GraphQLModule } from './graphql.module';
import { HttpClientModule } from '@angular/common/http';
import { SolNumberComponent } from './sol-number/sol-number.component';
import { WeatherDataComponent } from './weather-data/weather-data.component';
import { MarsPhotoComponent } from './mars-photo/mars-photo.component';

@NgModule({
  declarations: [
    AppComponent,
    SolNumberComponent,
    WeatherDataComponent,
    MarsPhotoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    GraphQLModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
