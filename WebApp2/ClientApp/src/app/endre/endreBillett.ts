import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Billett } from "../Billett";

@Component({
  templateUrl: "endreBillett.html"
})
export class EndreBillett {
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
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.endreKunde(params.id);
    })
  }

  vedSubmit() {
    this.endreEnKunde();
  }

  endreKunde(id: number) {
    this.http.get<Billett>("api/billett/" + id)
      .subscribe(
        billett => {
          this.skjema.patchValue({ id: billett.id });
          this.skjema.patchValue({ fornavn: billett.fornavn });
          this.skjema.patchValue({ etternavn: billett.etternavn });
          this.skjema.patchValue({ epost: billett.epost });
          this.skjema.patchValue({ mobilnummer: billett.mobilnummer });
        },
        error => console.log(error)
      );
  }

  endreEnKunde() {
    const endretBillett = new Billett();
    endretBillett.id = this.skjema.value.id;
    endretBillett.fornavn = this.skjema.value.fornavn;
    endretBillett.etternavn = this.skjema.value.etternavn;

    this.http.put("api/billett/", endretBillett)
      .subscribe(
        retur => {
          this.router.navigate(['/liste']);
        },
        error => console.log(error)
      );
  }
}
