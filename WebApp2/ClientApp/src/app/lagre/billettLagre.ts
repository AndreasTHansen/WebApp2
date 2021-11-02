import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Billett } from "../Billett";
import { Kunde } from "../Kunde";
import { Reise } from "../Reise"

@Component({
  templateUrl: "billettLagre.html"
})
export class BillettLagre {
  alleKunder: Array<Kunde>;
  reiseListe: Array<Reise>;
  skjema: FormGroup;

  hentAlleKunder() {
    this.http.get<Kunde[]>("api/kunde/")
      .subscribe(kundene => {
        this.alleKunder = kundene;
        this.laster = false;
      },
        error => console.log(error)
      );
  };

  validering = {
    id: [""],
    fornavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ -]{2,30}")])
    ],
    etternavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    mobilnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9\.\ \-]{8,12}")])
    ],
    epost: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  vedSubmit() {
    this.lagreKunde();
  }

  lagreKunde() {
    const lagretBillett = new Billett();

    lagretBillett.fornavn = this.skjema.value.fornavn;
    lagretBillett.etternavn = this.skjema.value.etternavn;
    lagretBillett.mobilnummer = this.skjema.value.mobilnummer;
    lagretBillett.epost = this.skjema.value.epost;

    this.http.post("api/billett", lagretBillett)
      .subscribe(retur => {
        this.router.navigate(['/liste']);
      },
        error => console.log(error)
      );
  };
}
