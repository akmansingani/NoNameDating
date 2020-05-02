import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
/** register component*/
export class RegisterComponent implements OnInit {

  model: any = {};
  @Output() closeRegister = new EventEmitter();
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  user: User;

/** register ctor */
  constructor(private authservice: AuthService, private alertService: AlertifyService,
    private fb: FormBuilder, private router: Router) {

  }

  ngOnInit() {

    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();

  }

  registerUser() {

    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authservice.registerService(this.user).subscribe((response) => {
        this.alertService.success('Registration Succesfully');
      }, error => {
          this.alertService.error(error);
      }, () => {
        this.authservice.loginService(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      });
    }

  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  cancel() {
    this.closeRegister.emit(false);
  }

}
