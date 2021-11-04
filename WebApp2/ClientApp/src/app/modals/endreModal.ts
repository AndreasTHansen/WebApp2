import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'endreModal.html'
})
export class EndreModal {
  constructor(public modal: NgbActiveModal) { }
}
