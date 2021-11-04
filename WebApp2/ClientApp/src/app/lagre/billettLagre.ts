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
    fornavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,30}")])
    ],
    etternavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    mobilnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9\.\ \-]{8,12}")])
    ],
    epost: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")])
    ],
    kortnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{16}")])
    ],
    utlopsdato: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9/]{10}")])
    ],
    cvc: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{3}")])
    ],
    datoAvreise: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9/]{10}")])
    ],
    datoAnkomst: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9/]{10}")])
    ],
 
    tidspunktFra: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9:/]{4,6}")])
    ],
    tidspunktTil: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9:/]{4,6}")])
    ],
    reiseFra: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    reiseTil: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    reisePris: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9.]{1,6}")])
    ],
    antallBarn: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{0,1}")])
    ],
    antallVoksne: [
      null, Validators.compose([Validators.required, Validators.pattern("[1-9]{1}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.skjema = fb.group(this.validering);
    
  }

  vedSubmit() {
    this.lagreBillett();
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

  lagreBillett() {
    const lagretBillett = new Billett();

    lagretBillett.fornavn = this.skjema.value.fornavn;
    lagretBillett.etternavn = this.skjema.value.etternavn;
    lagretBillett.mobilnummer = this.skjema.value.mobilnummer;
    lagretBillett.epost = this.skjema.value.epost;
    lagretBillett.kortnummer = this.skjema.value.kortnummer;
    lagretBillett.utlopsdato = this.skjema.value.utlopsdato;
    lagretBillett.cvc = this.skjema.value.cvc;
    lagretBillett.reiseFra = this.skjema.value.reiseFra;
    lagretBillett.reiseTil = this.skjema.value.reiseTil;
    lagretBillett.datoAvreise = this.skjema.value.datoAvreise;
    lagretBillett.datoAnkomst = this.skjema.value.datoAnkomst;
    lagretBillett.tidspunktFra = this.skjema.value.tidspunktFra;
    lagretBillett.tidspunktTil = this.skjema.value.tidspunktTil;
    lagretBillett.reisePris = this.skjema.value.reisePris;
    lagretBillett.antallBarn = this.skjema.value.antallBarn;
    lagretBillett.antallVoksne = this.skjema.value.antallVoksne;
    lagretBillett.totalPris = (lagretBillett.reisePris * lagretBillett.antallVoksne) + (lagretBillett.reisePris * lagretBillett.antallBarn * 0.5);

    this.http.post("api/billett", lagretBillett)
      .subscribe(retur => {
        this.router.navigate(['/billettListe']);
      },
        error => console.log(error)
      );
  };
}
