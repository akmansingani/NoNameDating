import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt'
import { environment } from '../../environments/environment';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
  currentUser: User;

  constructor(private http: HttpClient) {
  }

  loginService(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {

          const user = response;
          if (user) {
            localStorage.setItem('authtoken', user.token);
            localStorage.setItem('user', JSON.stringify(user.user));
            this.currentUser = user.user;
            this.decodeToken = this.jwtHelper.decodeToken(user.token);
          }

        })
      )
  }

  registerService(model: any) {

    //console.log("regist ==> " + model);
    return this.http.post(this.baseUrl + 'register', model);

  }

  loggedInService() {

    const token = localStorage.getItem('authtoken');
    return !this.jwtHelper.isTokenExpired(token);

  }

  setDecodeToken() {
    const token = localStorage.getItem('authtoken');
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.decodeToken = this.jwtHelper.decodeToken(token);
    }

    if (user) {
      this.currentUser = user;
      if (this.currentUser.photourl === null) {
        this.currentUser.photourl = "/assets/dummyImage.png";
      }
    }

  }


}
