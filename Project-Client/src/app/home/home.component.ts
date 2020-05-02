import { Component } from '@angular/core';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
/** home component*/
export class HomeComponent {

  varRegister = false;

/** home ctor */
    constructor() {

  }

  registerActive() {

    this.varRegister = !this.varRegister;

  }

  closeRegisterEvent(varRegister: boolean) {
    this.varRegister = varRegister;
  }

}
