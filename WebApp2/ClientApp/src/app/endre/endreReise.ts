import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Reise } from "../Reise";

@Component({
  templateUrl: "EndreReise.html"
})
export class EndreReise {
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

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.skjema = fb.group(this.validering);
  }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.visEndret(params.id);
    })
  }

  vedSubmit() {
    this.endreReise();
  }

  visEndret(id: number) {
    this.http.get<Reise>("api/reise/" + id)
      .subscribe(
        reise => {
          this.skjema.patchValue({ reiseTil: reise.reiseTil });
          this.skjema.patchValue({ reiseFra: reise.reiseFra });
          this.skjema.patchValue({ datoAnkomst: reise.datoAnkomst });
          this.skjema.patchValue({ datoAvreise: reise.datoAvreise });
          this.skjema.patchValue({ tidspunktFra: reise.tidspunktFra });
          this.skjema.patchValue({ tidspunktTil: reise.tidspunktTil });
          this.skjema.patchValue({ pris: reise.reisePris });
        },
        error => console.log(error)
      );
  }

  endreReise() {
    const endretReise = new Reise();

    endretReise.reiseFra = this.skjema.value.reiseFra;
    endretReise.reiseTil = this.skjema.value.reiseTil;
    endretReise.datoAnkomst = this.skjema.value.datoAnkomst;
    endretReise.datoAvreise = this.skjema.value.datoAvreise;
    endretReise.reisePris = this.skjema.value.pris;

    this.http.put("api/reise", endretReise)
      .subscribe(retur => {
        this.router.navigate(['/reiseListe']);
      },
        error => console.log(error)
      );
  };
}
