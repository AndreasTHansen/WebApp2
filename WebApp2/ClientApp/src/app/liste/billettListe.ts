import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Billett } from "../Billett";

@Component({
  templateUrl: "billettListe.html"
})
export class BillettListe {
  alleBilletter: Array<Billett>;
  laster: boolean;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.laster = true;
    this.hentAlleBilletter();
  }

  hentAlleBilletter() {
    this.http.get < Billett[] > ("api/billett/")
      .subscribe(billettene => {
        this.alleBilletter = billettene;
        this.laster = false;
      },
        error => console.log(error)
      );
  };

  sletteBillett(id: number) {
    this.http.delete("api/billett/" + id)
      .subscribe(retur => {
        this.hentAlleBilletter();
        this.router.navigate(['/liste']);
      },
        error => console.log(error)
      );
  };
}
