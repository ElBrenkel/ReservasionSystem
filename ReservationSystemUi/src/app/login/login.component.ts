import { APP_ID, Component, OnInit } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { LoginPayload } from '../interfaces/loginPayload';
import { LoginResponse } from '../interfaces/loginResponse';
import { ReservationSystemApiService } from '../reservation-system-api.service';

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  username = "";
  password = "";
  passwordIcon = "visibility";
  passwordInputType = "password";

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  matcher = new MyErrorStateMatcher();

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
  }

  togglePasswordVisibility(showPassword: boolean): void {
    this.passwordIcon = showPassword ? "visibility_off" : "visibility";
    this.passwordInputType = showPassword ? "text" : "password";
  }

  onLoginClicked() {
    const payload: LoginPayload = { username: this.username, password: this.password };
    this.api.login(payload)
      .then((r: LoginResponse) => {
        if (r) {
          localStorage["token"] = r.token;
          this.router.navigate([""]);
        }
      });
  }
}
