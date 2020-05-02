import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authservice: AuthService, private routeService: Router) {}

  canActivate(): boolean  {

    if (this.authservice.loggedInService()) {
      return true;
    }

    this.routeService.navigate(['/']);
    return false;

  }
  
}
