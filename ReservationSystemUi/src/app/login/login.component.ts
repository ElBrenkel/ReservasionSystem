import { APP_ID, Component, OnInit } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MyErrorStateMatcher } from '../common/my-error-state-matcher';
import { LoginPayload } from '../interfaces/loginPayload';
import { LoginResponse } from '../interfaces/loginResponse';
import { TextInputData } from '../interfaces/textInputData';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  payload: LoginPayload = {
    username: "",
    password: ""
  };

  errorMessage = "";

  inputFields: TextInputData[] = [{
    inputType: "text",
    label: "Email",
    prop: "username",
    control: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.maxLength(100)
    ]),
    placeholder: "name@domain.com"
  }, {
    inputType: "password",
    label: "Password",
    prop: "password",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(25),
      Validators.minLength(8)
    ]),
    placeholder: ""
  }];

  matcher = new MyErrorStateMatcher();

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
  }

  onLoginClicked() {
    this.api.login(this.payload)
      .then((r: LoginResponse) => {
        if (r) {
          localStorage["token"] = r.token;
          localStorage["user"] = this.payload.username;
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
