import { i18nMetaToJSDoc } from '@angular/compiler/src/render3/view/i18n/meta';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MyErrorStateMatcher } from '../common/my-error-state-matcher';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  username = "";
  password = "";
  passwordIcon = "visibility";
  passwordInputType = "password";
  confirmPassword = "";
  confirmPasswordIcon = "visibility";
  confirmPasswordInputType = "password";
  errorMessage = "";

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.maxLength(100)
  ]);

  passwordFormGroup = new FormGroup({
    password: new FormControl('', [
      Validators.required
    ]), confirmPassword: new FormControl('', [
      Validators.required
    ])
  }, { validators: [this.passwordMatchValidator] }
  );

  passwordMatchValidator(frm: FormGroup) {
    return frm.controls['password'].value ===
      frm.controls['confirmPassword'].value ? null : { 'mismatch': true };
  }

  confirmPasswrodError() {
    if (this.passwordFormGroup.controls.confirmPassword.hasError('required')) {
      return "Password confirmation is required";
    }
    else if (this.passwordFormGroup.errors?.mismatch) {
      return "Passwords do not match";
    }
    return null;
  }

  matcher = new MyErrorStateMatcher();


  debug() {
    console.log({ password: this.password, confirmPassword: this.confirmPassword, errors: this.passwordFormGroup.errors });
  }

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
  }

  togglePasswordVisibility(showPassword: boolean): void {
    this.passwordIcon = showPassword ? "visibility_off" : "visibility";
    this.passwordInputType = showPassword ? "text" : "password";
  }

  toggleConfirmPasswordVisibility(showPassword: boolean): void {
    this.confirmPasswordIcon = showPassword ? "visibility_off" : "visibility";
    this.confirmPasswordInputType = showPassword ? "text" : "password";
  }
}
