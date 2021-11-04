import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { Inject, ViewChild, ElementRef } from '@angular/core';
import { Bruker } from './Bruker';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Color Line Admin';

  constructor(private http: HttpClient, private router: Router, private builder: FormBuilder) { }

  @ViewChild('loggInnScreen', { static: true }) loggInnScreen: ElementRef;

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

    alert("Logger inn med " + bruker.brukernavn+" "+bruker.passord);

    this.http.post("api/bruker", bruker)
      .subscribe(retur => {
        //DETTE HER
        if (retur) {
          alert(retur);
          alert("Du logget inn");

          this.router.navigate(['/liste']);
          this.loggInnScreen.nativeElement.hidden = false;

          console.log(this.loginForm.value.brukernavn);
          console.log(this.loginForm.value.passord);
        }
        else {
          alert("Kom ikke inn");

          error => console.log(error)

        }
      },
        error => console.log("feil"+error.message)
      );


  };
}


