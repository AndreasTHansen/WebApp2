import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'feilModal.html'
})
export class FeilModal {
  constructor(public modal: NgbActiveModal) { }
}
