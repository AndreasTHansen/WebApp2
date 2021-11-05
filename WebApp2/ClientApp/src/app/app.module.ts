import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { LagreKunde } from './lagre/lagreKunde';
import { ListeKunde } from './liste/listeKunde';
import { EndreKunde } from './endre/endreKunde';
import { Meny } from './meny/meny';
import { BillettListe } from './liste/billettListe';
import { BillettEndre } from './endre/endreBillett';
import { ReiseListe } from "./liste/reiseListe";
import { LagreReise } from './lagre/lagreReise';
import { EndreReise } from './endre/endreReise';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SletteModal } from "./modals/sletteModal";
import { EndreModal } from "./modals/endreModal";

import { Login } from "./login/login";
import { MenyService } from './meny/meny.service';

@NgModule({
  declarations: [
    AppComponent,
    Meny,

    LagreKunde,  
    ListeKunde,
    EndreKunde,
    Login,

    BillettListe,
    BillettEndre,

    ReiseListe,
    LagreReise,
    EndreReise,

    SletteModal,
    EndreModal
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [MenyService],
  bootstrap: [AppComponent],
  entryComponents: [SletteModal, EndreModal]
})
export class AppModule { }
