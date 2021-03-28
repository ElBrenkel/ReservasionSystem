import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';
import { TextInputData } from '../interfaces/textInputData';
import { ReservationSystemApiService } from '../reservation-system-api.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from '../interfaces/confirmDialogData';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-room-view-header',
  templateUrl: './room-view-header.component.html',
  styleUrls: ['./room-view-header.component.scss']
})
export class RoomViewHeaderComponent implements OnInit {
  @Input() roomData: RoomData;
  @Input() isNew: boolean = false;
  @Output() editChanged: EventEmitter<any> = new EventEmitter();
  editIcon = "mode";
  isEditMode = false;
  get activationIcon(): string {
    return this.roomData && this.roomData.isActive ? "flash_off" : "flash_on";
  }

  roomNameInput: TextInputData = {
    inputType: "text",
    label: "",
    prop: "name",
    control: new FormControl('', []),
    placeholder: "Room name"
  };

  addressInputs: TextInputData[] = [{
    inputType: "text",
    label: "",
    prop: "street",
    control: new FormControl('', []),
    placeholder: "Street"
  }, {
    inputType: "number",
    label: "",
    prop: "buildingNumber",
    control: new FormControl('', []),
    placeholder: "Building number"
  }, {
    inputType: "text",
    label: "",
    prop: "city",
    control: new FormControl('', []),
    placeholder: "City"
  }];

  constructor(private api: ReservationSystemApiService, public dialog: MatDialog, public snackBar: MatSnackBar, private router: Router) { }

  ngOnInit(): void {
    if (this.isNew) {
      this.changeEditMode();
    }
  }

  calcPicNumber(): number {
    const seed = `${this.roomData.id}`;
    return Utils.calcPicNumber(seed);
  }

  roomAddress(): string {
    return Utils.roomAddress(this.roomData);
  }

  getlocationLink(): string {
    return `https://www.google.com/maps?q=${this.roomData.lat},${this.roomData.lon}`;
  }

  onLocationClick(): void {
    window.open(this.getlocationLink(), "_blank");
  }

  async onEditClick(): Promise<void> {
    if (this.isEditMode) {
      this.editIcon = "hourglass_empty";
      const success = await this.changeRoomData();
      if (success) {
        this.changeEditMode();
      }
      else {
        this.editIcon = "mode";
      }
    }
    else {
      this.changeEditMode();
    }
  }

  private changeEditMode(): void {
    this.isEditMode = !this.isEditMode;
    this.editIcon = this.isEditMode ? "done" : "mode";
    this.editChanged.emit(this.isEditMode);
  }

  roomName(): string {
    let name = this.roomData.name;
    if (!this.roomData.isActive) {
      name = name + " - Closed";
    }

    return name;
  }

  async addNewRoom(): Promise<boolean> {
    const payload = {
      id: this.roomData.id,
      name: this.roomData.name,
      country: "Israel",
      city: this.roomData.city,
      street: this.roomData.street,
      buildingNumber: this.roomData.buildingNumber,
      isActive: true,
      size: 10,
      maxNumberOfPeople: 10,
      workinghours: this.roomData.workingHours
    };

    const response = await this.api.addRoomData(payload);
    if (response.status.success) {
      this.router.navigate([`room/${response.object.id}`]);
    }
    else {
      console.log(response);
    }

    return response.status.success;
  }

  async changeRoomData(): Promise<boolean> {
    if (this.isNew) {
      return await this.addNewRoom();
    }
    const roomEditSuccess = await this.editRoomData();
    const workingHoursEditSuccess = await this.editWorkingHours();
    return roomEditSuccess && workingHoursEditSuccess;
  }

  async editWorkingHours(): Promise<boolean> {
    console.log(this.roomData.workingHours);
    const response = await this.api.editWorkingHours(this.roomData.id, this.roomData.workingHours);
    if (response.status.success) {
      this.roomData.workingHours = response.object;
    }
    else {
      Utils.openSnackbar(this.snackBar, false, response.status.message);
    }

    return response.status.success;
  }

  async editRoomData(): Promise<boolean> {
    const payload = {
      id: this.roomData.id,
      name: this.roomData.name,
      street: this.roomData.street,
      buildingNumber: this.roomData.buildingNumber,
      city: this.roomData.city
    };

    const response = await this.api.editRoomData(payload);
    if (response.status.success) {
      this.roomData.name = response.object.name;
      this.roomData.street = response.object.street;
      this.roomData.buildingNumber = response.object.buildingNumber;
      this.roomData.city = response.object.city;
    }
    else {
      console.log(response);
    }

    return response.status.success;
  }

  async onActivationChanged(): Promise<void> {
    if (this.roomData.isActive) {
      const response = await this.api.deactivateRoom(this.roomData.id, false);
      if (response.message === "Could not complete operation, room has future reservations.") {
        const data: ConfirmDialogData = { title: "Are you sure?", message: "There are other active reservations. Proceed anyway?" }
        const dialogRef = this.dialog.open(ConfirmDialogComponent, { width: "400px", data });
        dialogRef.afterClosed().subscribe(async result => {
          const forcedResponse = await this.api.deactivateRoom(this.roomData.id, true);
          if (forcedResponse.success) {
            this.roomData.isActive = false;
          }
        });
      }
      else if (response.success) {
        this.roomData.isActive = false;
      }
    }
    else {
      const respose = await this.api.activateRoom(this.roomData.id);
      if (respose.success) {
        this.roomData.isActive = true;
      }
    }
  }
}
