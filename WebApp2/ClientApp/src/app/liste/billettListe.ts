import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Billett } from "../Billett";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SletteModal } from "../modals/sletteModal";

@Component({
  templateUrl: "billettListe.html"
})
export class BillettListe {
  alleBilletter: Array<Billett>;
  laster: boolean;
  billettTilSletting: string;

  constructor(private http: HttpClient, private router: Router, private modalService: NgbModal) { }

  ngOnInit() {
    this.laster = true;
    this.hentAlleBilletter();
  }

  hentAlleBilletter() {
    this.http.get < Billett[] > ("api/billett/")
      .subscribe(billettene => {
        this.alleBilletter = billettene;
        this.laster = false;
      },
        error => console.log(error)
      );
  };

  sletteBillett(id: number) {

    this.http.get<Billett>("api/billett/" + id)
      .subscribe(billett => {
        this.billettTilSletting = billett.id + ",  " + billett.fornavn + " " + billett.etternavn + ",  " + billett.reiseFra + " - " + billett.reiseTil;

        this.visOgSlett(id);
      },
        error => console.log(error)
      );
  };

  visOgSlett(id: number) {

    const sletteModal = this.modalService.open(SletteModal)
    sletteModal.componentInstance.sletteObjekt = this.billettTilSletting;

    sletteModal.result.then(retur => {
      console.log("Lukket med" + retur)
      if (retur == "Slett") {

        this.http.delete("api/billett/" + id)
          .subscribe(retur => {
            this.hentAlleBilletter();
          },
            error => console.log(error)
          );
      }
      this.router.navigate(['/billettListe']);
    });
  }
}
