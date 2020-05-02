import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { error } from 'protractor';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {}

  constructor(public authservice: AuthService, private alertService: AlertifyService, private routeService: Router) { }

  ngOnInit(): void {
  }

  loginUser() {
    this.authservice.loginService(this.model).subscribe(next => {
      this.alertService.success('Log in success');
      this.routeService.navigate(['/members']);
    }, error => {
        this.alertService.error(error);
    });
  }

  isLogin() {
   // const token = localStorage.getItem('authtoken');
    // return !!token;

    return this.authservice.loggedInService();

  }

  logoutUser() {

    localStorage.removeItem('authtoken');
    localStorage.removeItem('user');
    this.authservice.decodeToken = null;
    this.authservice.currentUser = null;
    this.alertService.success('Logout Succesfully');
    this.model = {};
    this.routeService.navigate(['/home']);
  }

}
