import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Utils } from '../common/utils';
import { User } from '../interfaces/user';
import { ReservationSystemApiService } from '../reservation-system-api.service';
import { cloneDeep } from "lodash";

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {
  @Input() user: User;
  userBackUp: User;

  constructor(private api: ReservationSystemApiService, public snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.userBackUp = cloneDeep(this.user);
  }

  async onValueChanged(prop: string): Promise<void> {
    const payload = {
      [prop]: prop == "buildingNumber" ? this.user[prop] * 1 : this.user[prop]
    }

    const response = await this.api.changeUserData(payload);
    if (response.status.success) {
      this.user = response.object;
      this.userBackUp = cloneDeep(this.user);
    }
    else {
      this.user = cloneDeep(this.userBackUp);
      Utils.openSnackbar(this.snackBar, false, response.status.message);
    }
  }
}
