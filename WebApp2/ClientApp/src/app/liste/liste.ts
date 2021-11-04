import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Kunde } from "../Kunde";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SletteModal } from "../modals/sletteModal";

import { MenyService } from '../meny/meny.service';

@Component({
  templateUrl: "liste.html"
})
export class Liste implements OnInit{
  alleKunder: Array<Kunde>;
  laster: boolean;
  kundeTilSletting: string;

  constructor(public nav: MenyService, private http: HttpClient, private router: Router, private modalService: NgbModal) {}

  ngOnInit() {
    this.nav.show();
    this.laster = true;
    this.hentAlleKunder();
  }

  hentAlleKunder() {
    this.http.get<Kunde[]>("api/kunde/")
      .subscribe(kundene => {
        this.alleKunder = kundene;
        this.laster = false;
      },
       error => console.log(error)
      );
  };

  sletteKunde(id: number) {

    this.http.get<Kunde>("api/kunde/" + id)
      .subscribe(kunde => {
        this.kundeTilSletting = kunde.fornavn + " " + kunde.etternavn;

        this.visOgSlett(id);
      },
        error => console.log(error)
      );
  };

  visOgSlett(id: number) {

    const sletteModal = this.modalService.open(SletteModal)
    sletteModal.componentInstance.sletteObjekt = this.kundeTilSletting;

    sletteModal.result.then(retur => {
      console.log("Lukket med" + retur)
      if (retur == "Slett") {

        this.http.delete("api/kunde/" + id)
          .subscribe(retur => {
            this.hentAlleKunder();
          },
            error => alert("Denne reisen kan ikke slettes fordi den finnes i en billett")
          );
      }
      this.router.navigate(['/liste']);
    });
  }
}
