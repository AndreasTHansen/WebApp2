import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Reise } from "../Reise";

@Component({
  templateUrl: "lagreReise.html"
})
export class LagreReise {
  skjema: FormGroup;

  validering = {
    id: [""],
    reiseFra: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ -]{2,50}")])
    ],
    reiseTil: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    datoAnkomst: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9./\:]{2,20}$")])
    ],
    tidspunktFra: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9:]{2,20}$")])
    ],
    tidspunktTil: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9:]{2,20}")])
    ],
    datoAvreise: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9./\:]{2,20}")])
    ],
    pris: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{2,8}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  vedSubmit() {
    this.lagreReise();
  }

  lagreReise() {
    const lagretReise = new Reise();

    lagretReise.reiseFra = this.skjema.value.reiseFra;
    lagretReise.reiseTil = this.skjema.value.reiseTil;
    lagretReise.datoAnkomst = this.skjema.value.datoAnkomst;
    lagretReise.datoAvreise = this.skjema.value.datoAvreise;
    lagretReise.reisePris = this.skjema.value.pris;

    this.http.post("api/reise", lagretReise)
      .subscribe(retur => {
        this.router.navigate(['/reiseListe']);
      },
        error => console.log(error)
      );
  };
}
