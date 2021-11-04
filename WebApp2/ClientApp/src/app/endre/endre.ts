import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Kunde } from "../Kunde";
import { EndreModal } from "../modals/endreModal";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: "endre.html"
})
export class Endre {
  skjema: FormGroup;
  endretKunde: string;
  
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
    kortnummer: [""]
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router, private modalService: NgbModal) {
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
    this.http.get<Kunde>("api/kunde/" + id)
      .subscribe(
        kunde => {
          this.skjema.patchValue({ id: kunde.id });
          this.skjema.patchValue({ fornavn: kunde.fornavn });
          this.skjema.patchValue({ etternavn: kunde.etternavn });
          this.skjema.patchValue({ epost: kunde.epost });
          this.skjema.patchValue({ mobilnummer: kunde.mobilnummer });
          this.skjema.patchValue({ kortnummer: kunde.kortnummer });
        },
        error => console.log(error)
      );
  }

  endreEnKunde() {
    const endretKunde = new Kunde();
    endretKunde.id = this.skjema.value.id;
    endretKunde.fornavn = this.skjema.value.fornavn;
    endretKunde.etternavn = this.skjema.value.etternavn;
    endretKunde.epost = this.skjema.value.epost;
    endretKunde.mobilnummer = this.skjema.value.mobilnummer;
    endretKunde.kortnummer = this.skjema.value.kortnummer;
   

    this.http.put("api/kunde/", endretKunde)
      .subscribe(
        retur => {
          this.endretKunde = endretKunde.fornavn + "  " + endretKunde.etternavn;
          const endreModal = this.modalService.open(EndreModal)
          endreModal.componentInstance.endreObjekt = this.endretKunde;
          this.router.navigate(['/liste']);
        },
        error => console.log(error)
      );
  }
}
