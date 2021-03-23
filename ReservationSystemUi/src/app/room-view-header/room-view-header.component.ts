import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';
import { TextInputData } from '../interfaces/textInputData';
import { ReservationSystemApiService } from '../reservation-system-api.service';
import { debounce } from "lodash";

@Component({
  selector: 'app-room-view-header',
  templateUrl: './room-view-header.component.html',
  styleUrls: ['./room-view-header.component.scss']
})
export class RoomViewHeaderComponent implements OnInit {
  @Input() roomData: RoomData;
  @Output() editChanged: EventEmitter<any> = new EventEmitter();
  editIcon = "mode";
  isEditMode = false;

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

  constructor(private api: ReservationSystemApiService) { }

  ngOnInit(): void {
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

  async changeRoomData(): Promise<boolean> {
    const payload = {
      id: this.roomData.id,
      name: this.roomData.name,
      street: this.roomData.street,
      buildingNumber: this.roomData.buildingNumber,
      city: this.roomData.city
    };

    const response = await this.api.editRoomData(payload);
    if (response.status.success) {
      this.roomData = response.object;
    }
    else {
      console.log(response);
    }

    return response.status.success;
  }
}
