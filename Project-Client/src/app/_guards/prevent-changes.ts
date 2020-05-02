import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';




@Injectable({
  providedIn: 'root'
})
export class PreventChangesGuard implements CanDeactivate<MemberEditComponent> {

  constructor() { }

  canDeactivate(component: MemberEditComponent) {

    if (component.editForm.dirty) {
      return confirm("Are you sure you want to leave this page?, changes will be lost!");
    }

    return true;

  }

}
