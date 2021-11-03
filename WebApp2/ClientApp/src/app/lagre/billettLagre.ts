import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Billett } from "../Billett";
import { Kunde } from "../Kunde";
import { Reise } from "../Reise"

import { Inject, ViewChild, ElementRef } from '@angular/core';

@Component({
  templateUrl: "billettLagre.html"
})
export class BillettLagre {
  alleKunder: Array<Kunde>;
  alleReiser: Array<Reise>;

  skjema: FormGroup;
  laster: boolean;

  valgtKunde: Kunde;

  @ViewChild('kundeListe', { static: true }) kundeListe: ElementRef;
 

  ngOnInit() {
    this.laster = true;
    this.hentAlleKunder();
    this.hentAlleReiser();
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

  hentAlleReiser() {
    this.http.get<Reise[]>("api/reise/")
      .subscribe(reisene => {
        this.alleReiser = reisene;
        this.laster = false;
      },
        error => console.log(error)
      );
  };

  validering = {
    id: [""],
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

  visKundeListe() {
    this.kundeListe.nativeElement.hidden = false;
  }

  setKunde(id) {

    for (let kunde of this.alleKunder) {
      if (kunde.id == id) {
        this.valgtKunde = kunde;
      };
    };

  }

  lagreKunde() {
    const lagretBillett = new Billett();

    lagretBillett.fornavn = this.valgtKunde.fornavn;
    lagretBillett.kundeId = this.valgtKunde.id;
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
