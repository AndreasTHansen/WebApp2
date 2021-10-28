import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Kunde } from "../Kunde";

@Component({
  templateUrl: "lagre.html"
})
export class Lagre {
  skjema: FormGroup;
  
  validering = {
    id: [""],
    fornavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    etternavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    mobilnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    epost: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{4}")])
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

    this.http.post("api/kunde", lagretKunde)
      .subscribe(retur => {
        this.router.navigate(['/liste']);
      },
       error => console.log(error)
      );
  };
}
