import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Lagre } from './lagre/lagre';
import { Liste } from './liste/liste';
import { Endre } from './endre/endre';
import { BillettListe } from './liste/billettListe';
import { BillettEndre } from './endre/endreBillett';
import { ReiseListe } from "./liste/reiseListe";
import { LagreReise } from './lagre/lagreReise';
import { EndreReise } from './endre/endreReise';
import { Login } from './login/login';

const appRoots: Routes = [
  { path: 'lagre', component: Lagre },
  { path: 'liste', component: Liste },
  { path: 'endre/:id', component: Endre },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'billettListe', component: BillettListe },
  { path: 'endreBillett/:id', component: BillettEndre },
  { path: 'reiseListe', component: ReiseListe },
  { path: 'lagreReise', component: LagreReise },
  { path: 'endreReise/:id', component: EndreReise },
  { path: 'login', component: Login }

]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoots)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
