import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Kunde } from "../Kunde";

@Component({
  templateUrl: "lagreKunde.html"
})
export class LagreKunde {
  skjema: FormGroup;
  
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
    ],
    kortnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{16}")])
    ],
    utlopsdato: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{2}[\/][0-9]{2}[\/][0-9]{4}")])
    ],
    cvc: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{3}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  vedSubmit() {
      this.lagreKunde();
  }

  lagreKunde() {
    const lagretKunde = new Kunde();

    lagretKunde.fornavn = this.skjema.value.fornavn;
    lagretKunde.etternavn = this.skjema.value.etternavn;
    lagretKunde.mobilnummer = this.skjema.value.mobilnummer;
    lagretKunde.epost = this.skjema.value.epost;
    lagretKunde.kortnummer = this.skjema.value.kortnummer;
    lagretKunde.utlopsdato = this.skjema.value.utlopsdato;
    lagretKunde.cvc = this.skjema.value.cvc;

    this.http.post("api/kunde", lagretKunde)
      .subscribe(retur => {
        this.router.navigate(['/listeKunde']);
      },
       error => console.log(error)
      );
  };
}
