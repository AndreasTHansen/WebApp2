import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Kunde } from "../Kunde";

@Component({
  templateUrl: "liste.html"
})
export class Liste {
  alleKunder: Array<Kunde>;
  laster: boolean;

  constructor(private http: HttpClient,private router: Router) { }

  ngOnInit() {
    this.laster = true;
    this.hentAlleKunder();
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

  sletteKunde(id: number) {
    this.http.delete("api/kunde/" + id)
      .subscribe(retur => {
        this.hentAlleKunder();
        this.router.navigate(['/liste']);
      },
       error => console.log(error)
      );
  };
}
