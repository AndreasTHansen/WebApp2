import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Reise } from "../Reise";
import { error } from "protractor";


@Component({
    templateUrl: "reiseListe.html"
})


export class ReiseListe {
    alleReiser: Array<Reise>;
    laster: boolean;

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleReiser();
    }

    hentAlleReiser() {
        this.http.get<Reise[]>("api/reise/")
            .subscribe(reisene => {
                this.alleReiser = reisene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };

    sletteReise(id: number) {
        this.http.delete("api/reise/" + id)
            .subscribe(retur => {
                this.hentAlleReiser();
                this.router.navigate(['/reiseiste']);
            },
              error => console.log(error)
            );
    };
}
