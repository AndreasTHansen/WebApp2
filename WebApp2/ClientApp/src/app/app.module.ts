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
import { BillettEndre } from './endre/endreBillett';
import { ReiseListe } from "./liste/reiseListe";
import { LagreReise } from './lagre/lagreReise';
import { EndreReise } from './endre/endreReise';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SletteModal } from "./modals/sletteModal";
import { FeilModal } from "./modals/feilModal";

import { Login } from "./login/login";

@NgModule({
  declarations: [
    AppComponent,
    Meny,

    Lagre,  
    Liste,
    Endre,
    Login,

    BillettListe,
    BillettLagre,
    BillettEndre,

    ReiseListe,
    LagreReise,
    EndreReise,

    SletteModal,
    FeilModal
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [SletteModal, FeilModal]
})
export class AppModule { }
