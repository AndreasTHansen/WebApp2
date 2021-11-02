import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { Lagre } from './lagre/lagre';
import { Liste } from './liste/liste';
import { Endre } from './endre/endre';
import { Meny } from './meny/meny';
import { BillettListe } from './liste/billettListe';
import { BillettLagre } from './lagre/billettLagre';
import { ReiseListe } from "./liste/reiseListe";
import { LagreReise } from './lagre/lagreReise';
import { EndreReise } from './endre/endreReise';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    Meny,

    Lagre,  
    Liste,
    Endre,

    BillettListe,
    BillettLagre,
    BillettListe,

    ReiseListe,
    LagreReise,
    EndreReise
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
