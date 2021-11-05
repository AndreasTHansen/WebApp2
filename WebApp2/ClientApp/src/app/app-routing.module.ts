import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LagreKunde } from './lagre/lagreKunde';
import { ListeKunde } from './liste/listeKunde';
import { EndreKunde } from './endre/endreKunde';
import { BillettListe } from './liste/billettListe';
import { BillettEndre } from './endre/endreBillett';
import { ReiseListe } from "./liste/reiseListe";
import { LagreReise } from './lagre/lagreReise';
import { EndreReise } from './endre/endreReise';
import { Login } from './login/login';

const appRoots: Routes = [
  { path: 'lagreKunde', component: LagreKunde },
  { path: 'listeKunde', component: ListeKunde },
  { path: 'endreKunde/:id', component: EndreKunde },
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
