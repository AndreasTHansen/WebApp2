import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { ViewChild, ElementRef } from '@angular/core';
import { Bruker } from '../Bruker';
import { MenyService } from '../meny/meny.service';

@Component({
  selector: 'app-root',
  templateUrl: './login.html'
})
export class Login implements OnInit{
  title = 'Login';

  constructor(public nav: MenyService, private http: HttpClient, private router: Router, private builder: FormBuilder) { }

  @ViewChild('loggInnScreen', { static: true }) loggInnScreen: ElementRef;

  ngOnInit() {
    this.nav.hide();
  }

  loginForm: FormGroup = this.builder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  vedSubmit() {
    this.loggInn();
  }

  loggInn() {
    const bruker = new Bruker();
    bruker.brukernavn = this.loginForm.value.username;
    bruker.passord = this.loginForm.value.password;

    this.http.post("api/bruker", bruker)
      .subscribe(retur => {
        //DETTE HER
        if (retur) {
          alert("Du logget inn");

          this.router.navigate(['/listeKunde']);
        }
        else {
          alert("Kom ikke inn");

          error => console.log(error)

        }
      },
        error => console.log("feil" + error.message)
      );
  };
}




