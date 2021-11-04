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

    alert("Logger inn med " + bruker.brukernavn + " " + bruker.passord);

    this.http.post("api/bruker", bruker)
      .subscribe(retur => {
        //DETTE HER
        if (retur) {
          alert(retur);
          alert("Du logget inn");

          this.router.navigate(['/liste']);

          console.log(this.loginForm.value.brukernavn);
          console.log(this.loginForm.value.passord);
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




