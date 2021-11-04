import { Component } from '@angular/core';
import { MenyService } from './meny.service'
import { HttpClient } from '@angular/common/http';


@Component({
  moduleId: module.id,
  selector: 'app-nav-meny',
  templateUrl: './meny.html'
})

export class Meny {

  constructor(public nav: MenyService, private http: HttpClient) { };

  loggUt() {
    this.http.get("api/bruker")
      .subscribe(retur => {
        if (retur) {
          alert(retur);
        }
        else {
          alert("Fikk ikke logget ut")

          error => console.log(error)
        }
      },
        error => console.log("feil" + error.message)
      );
  };

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

}
