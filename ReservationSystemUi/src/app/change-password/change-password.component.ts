import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Utils } from '../common/utils';
import { ChangePassword } from '../interfaces/changePassword';
import { TextInputData } from '../interfaces/textInputData';
import { User } from '../interfaces/user';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  @Input() user: User;
  payload: ChangePassword = {};

  inputFields: TextInputData[] = [{
    inputType: "password",
    label: "Current password",
    prop: "currentPassword",
    control: new FormControl('', []),
    placeholder: ""
  }, {
    inputType: "password",
    label: "New password",
    prop: "newPassword",
    control: new FormControl('', []),
    placeholder: ""
  }, {
    inputType: "password",
    label: "Confirm new password",
    prop: "newPasswordAgain",
    control: new FormControl('', []),
    placeholder: ""
  }];

  constructor(private api: ReservationSystemApiService, public snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  async onPasswordChangedClicked(): Promise<void> {
    const response = await this.api.changePassword(this.payload);
    if (response.success) {
      Utils.openSnackbar(this.snackBar, true, "Password changed successfully");
      this.payload = {};
    }
    else {
      Utils.openSnackbar(this.snackBar, false, response.message);
    }
  }
}
