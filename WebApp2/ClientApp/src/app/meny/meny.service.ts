import { Injectable } from '@angular/core';

@Injectable()
export class MenyService {
  visible: boolean;

  constructor() { this.visible = true; }

  hide() { this.visible = false; }

  show() { this.visible = true; }

  toggle() {
    this.visible = !this.visible;
    this.isExpanded = !this.isExpanded;
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

}
