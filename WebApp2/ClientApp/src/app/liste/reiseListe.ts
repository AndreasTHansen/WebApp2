import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SletteModal } from "../modals/sletteModal";
import { Reise } from "../Reise";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { error } from "protractor";
import { FeilModal } from "../modals/feilModal";


@Component({
    templateUrl: "reiseListe.html"
})


export class ReiseListe {
    alleReiser: Array<Reise>;
    laster: boolean;
  reiseTilSletting: string;
  sletteOK: boolean;

    constructor(private http: HttpClient, private router: Router, private modalService: NgbModal) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleReiser();
    }

    hentAlleReiser() {
        this.http.get<Reise[]>("api/reise/")
            .subscribe(reisene => {
                this.alleReiser = reisene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };

    sletteReise(id: number) {

      this.http.get<Reise>("api/reise/" + id)
        .subscribe(reise => {
          this.reiseTilSletting = reise.reiseFra + " - " + reise.reiseTil;

          this.visOgSlett(id);
      },
         error => console.log(error)
      );
  };

  visOgSlett(id: number) {

    const sletteModal = this.modalService.open(SletteModal)
    sletteModal.componentInstance.sletteObjekt = this.reiseTilSletting;

    sletteModal.result.then(retur => {
      console.log("Lukket med" + retur)
      if (retur == "Slett") {

        this.http.delete("api/reise/" + id)
          .subscribe(retur => {
            this.hentAlleReiser();
          },
            error => alert("Denne reisen kan ikke slettes fordi den finnes i en billett")
          );
      }
      this.router.navigate(['/reiseListe']);
    });   
  }
}
