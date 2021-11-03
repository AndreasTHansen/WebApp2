import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Billett } from "../Billett";

@Component({
  templateUrl: "endreBillett.html"
})
export class BillettEndre {
  skjema: FormGroup;
  billetten: Billett;

  validering = {
    id: [""],
    kortnummer: [""],
    antallBarn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ -]{2,30}")])
    ],
    antallVoksne: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ -]{2,50}")])
    ],
    mobilnummer: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9\.\ \-]{8,12}")])
    ],
    epost: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")])
    ],
    totalPris: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9.]{8,12}")])
    ]
    
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.hentUtBillettSkjema(params.id);
    })
  }

  vedSubmit() {
    this.endreBillett();
  }

  hentUtBillettSkjema(id: number) {
    this.http.get<Billett>("api/billett/" + id)
      .subscribe(
        billett => {
          this.billetten = billett;
          this.skjema.patchValue({ id: billett.id });
          this.skjema.patchValue({ antallBarn: billett.antallBarn });
          this.skjema.patchValue({ antallVoksne: billett.antallVoksne });
          this.skjema.patchValue({ epost: billett.epost });
          this.skjema.patchValue({ mobilnummer: billett.mobilnummer });
        },
        error => console.log(error)
      );
  }

  endreBillett() {
    const endretBillett = new Billett();
    endretBillett.id = this.skjema.value.id;
    endretBillett.fornavn = this.skjema.value.fornavn;
    endretBillett.etternavn = this.skjema.value.etternavn;

    this.http.put("api/billett/", endretBillett)
      .subscribe(
        retur => {
          this.router.navigate(['/billettListe']);
        },
        error => console.log(error)
      );
  }
}
