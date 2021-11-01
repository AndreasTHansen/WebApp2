import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Lagre} from './lagre/lagre';
import { Liste } from './liste/liste';
import { Endre } from './endre/endre';
import { BillettListe } from './liste/billettListe';

const appRoots: Routes = [
  { path: 'lagre', component: Lagre },
  { path: 'liste', component: Liste },
  { path: 'endre/:id', component: Endre, },
  { path: '', redirectTo: '/liste', pathMatch: 'full' },
  { path: 'billettListe', component: BillettListe }
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
