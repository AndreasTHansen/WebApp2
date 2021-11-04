import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Billett } from "../Billett";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { EndreModal } from "../modals/endreModal";

@Component({
  templateUrl: "endreBillett.html"
})
export class BillettEndre {
  skjema: FormGroup;
  billetten: Billett;
  billettTilEndring: string;

  validering = {
    id: [""],
    kortnummer: [""],
    kundeId: [""],
    reiseId: [""],
    antallBarn: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{0,9}")])
    ],
    antallVoksne: [
      null, Validators.compose([Validators.required, Validators.pattern("[1-9]{1,9}")])
    ],
    totalPris: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9.]{1,6}")])
    ]
    
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router, private modalService: NgbModal) {
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
          this.skjema.patchValue({ kundeId: billett.kundeId });
          this.skjema.patchValue({ kortnummer: billett.kortnummer });
          this.skjema.patchValue({ reiseId: billett.reiseId });
          this.skjema.patchValue({ antallBarn: billett.antallBarn });
          this.skjema.patchValue({ antallVoksne: billett.antallVoksne });
          this.skjema.patchValue({ totalPris: billett.totalPris });
        },
        error => console.log(error)
      );
  }

  endreBillett() {
    const endretBillett = new Billett();
    endretBillett.id = this.skjema.value.id;
    endretBillett.kundeId = this.skjema.value.kundeId;
    endretBillett.kortnummer = this.skjema.value.kortnummer;
    endretBillett.reiseId = this.skjema.value.reiseId;
    endretBillett.antallBarn = this.skjema.value.antallBarn;
    endretBillett.antallVoksne = this.skjema.value.antallVoksne;
    endretBillett.totalPris = this.skjema.value.totalPris; 


    this.http.put("api/billett/", endretBillett)
      .subscribe(
        retur => {
          this.billettTilEndring = "Billett-id:  " + endretBillett.id;
          const endreModal = this.modalService.open(EndreModal)
          endreModal.componentInstance.endreObjekt = this.billettTilEndring;
          this.router.navigate(['/billettListe']);
        },
        error => console.log(error)
      );

  }
}
