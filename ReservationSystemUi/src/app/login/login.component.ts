import { APP_ID, Component, OnInit } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MyErrorStateMatcher } from '../common/my-error-state-matcher';
import { LoginPayload } from '../interfaces/loginPayload';
import { LoginResponse } from '../interfaces/loginResponse';
import { ReservationSystemApiService } from '../reservation-system-api.service';

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
  errorMessage = "";

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
          localStorage["user"] = this.username;
          this.router.navigate([""]);
        }
        else {
          this.errorMessage = "Wrong username or password.";
        }
      });
  }

  onRegisterClicked() {
    this.router.navigate(["register"]);
  }
}
