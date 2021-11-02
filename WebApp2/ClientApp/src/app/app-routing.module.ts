import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Lagre } from './lagre/lagre';
import { LagreReise } from './lagre/lagreReise';
import { Liste } from './liste/liste';
import { Endre } from './endre/endre';
import { BillettListe } from './liste/billettListe';
import { BillettLagre } from './lagre/billettLagre';
import { ReiseListe } from "./liste/reiseListe";

const appRoots: Routes = [
  { path: 'lagre', component: Lagre },
  { path: 'lagreReise', component: LagreReise },
  { path: 'liste', component: Liste },
  { path: 'endre/:id', component: Endre },
  { path: '', redirectTo: '/liste', pathMatch: 'full' },
  { path: 'billettListe', component: BillettListe },
  {path: 'billettLagre', component: BillettLagre},
  { path: 'reiseListe', component: ReiseListe },
  { path: '', redirectTo: '/liste', pathMatch: 'full' }
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
