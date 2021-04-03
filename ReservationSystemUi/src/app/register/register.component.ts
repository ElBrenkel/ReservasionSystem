import { i18nMetaToJSDoc } from '@angular/compiler/src/render3/view/i18n/meta';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MyErrorStateMatcher } from '../common/my-error-state-matcher';
import { GenericStatusMessage } from '../interfaces/genericStatusMessage';
import { RegisterPayload } from '../interfaces/registerPayload';
import { TextInputData } from '../interfaces/textInputData';
import { DefaultRoles, UserRole } from '../interfaces/userRole';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerPayload: RegisterPayload = {
    username: "",
    password: "",
    confirmPassword: "",
    firstName: "",
    lastName: "",
    country: "",
    city: "",
    street: "",
    buildingNumber: 0
  };

  userRoles: UserRole[] = DefaultRoles.ALL_ROLES;
  role = 1;

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
  }, {
    inputType: "text",
    label: "First name",
    prop: "firstName",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
      Validators.minLength(2)
    ]),
    placeholder: "John"
  }, {
    inputType: "text",
    label: "Last name",
    prop: "lastName",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
      Validators.minLength(2)
    ]),
    placeholder: "Doe"
  }, {
    inputType: "text",
    label: "Country",
    prop: "country",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
      Validators.minLength(2)
    ]),
    placeholder: "Israel"
  }, {
    inputType: "text",
    label: "City",
    prop: "city",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
      Validators.minLength(2)
    ]),
    placeholder: "Tel Aviv"
  }, {
    inputType: "text",
    label: "Street",
    prop: "street",
    control: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(2)
    ]),
    placeholder: "Waitzman"
  }, {
    inputType: "number",
    label: "Building number",
    prop: "buildingNumber",
    control: new FormControl('', [
      Validators.required,
      Validators.min(1)
    ]),
    placeholder: ""
  }
  ];

  errorMessage = "";

  userRoleFormControl = new FormControl('', [
    Validators.required
  ]);

  matcher = new MyErrorStateMatcher();

  onRegister() {
    this.registerPayload.confirmPassword = this.registerPayload.password;
    this.api.register(this.registerPayload, this.role)
      .then((r: GenericStatusMessage) => {
        if (r && r.success) {
          this.router.navigate(["login"]);
        }
        else {
          this.errorMessage = r.message;
        }
      });
  }

  onBback() {
    this.router.navigate(["login"]);
  }

  onClear() {
    this.registerPayload = {
      username: "",
      password: "",
      confirmPassword: "",
      firstName: "",
      lastName: "",
      country: "",
      city: "",
      street: "",
      buildingNumber: 0
    }
  }

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
  }
}
