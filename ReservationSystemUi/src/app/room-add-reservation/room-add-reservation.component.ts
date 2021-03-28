import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Utils } from '../common/utils';
import { AvailableTimes } from '../interfaces/availableTimes';
import { RoomData } from '../interfaces/roomData';
import { ReservationStatusSnackbarComponent } from '../reservation-status-snackbar/reservation-status-snackbar.component';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-room-add-reservation',
  templateUrl: './room-add-reservation.component.html',
  styleUrls: ['./room-add-reservation.component.scss']
})
export class RoomAddReservationComponent implements OnInit {
  @Input() roomData: RoomData;
  times = [];
  slots = [];
  date: Date | null;
  duration: number = 0;
  selectedSlot: AvailableTimes;
  dateFormControl = new FormControl(new Date());

  workingHoursFilter = (d: Date | null): boolean => {
    const workingHoursDays = this.roomData.workingHours.map(x => x.day);
    const day = (d || new Date()).getDay() + 1;
    return workingHoursDays.includes(day);
  }

  constructor(private api: ReservationSystemApiService, public snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.populateTimes();
  }

  onDateChange(event: any): void {
    this.date = event.value;
    this.populateSlots();
  }

  onDurationChange(event: any): void {
    this.duration = event.value;
    this.populateSlots();
  }

  async populateSlots(): Promise<void> {
    this.slots = [];
    if (!this.date || !this.duration) {
      return;
    }

    const availableTimes = await this.api.getAvailableTimes(this.roomData.id, this.date, this.duration);
    if (availableTimes.status.success) {
      this.slots = availableTimes.items.map(x => {
        return {
          value: x,
          viewValue: `${(new Date(x.rentStart)).toString()} - ${x.price} ₪`
        };
      });
      this.selectedSlot = this.slots[0].value;
    }
  }

  populateTimes(): void {
    this.times = [];
    for (let i = 0; i < 301; i += 30) {
      this.times.push({
        value: i,
        viewValue: Utils.getHour(i)
      });
    }
  }

  getFinalReservation(): string {
    const rentStart = new Date(this.selectedSlot.rentStart);
    const rentEnd = Utils.addMinutes(rentStart, this.duration);
    const date = rentStart.toDateString();
    const timeStart = Utils.getHourFromDate(rentStart);
    const timeEnd = Utils.getHourFromDate(rentEnd);
    const totalPrice = `${this.selectedSlot.price} ₪`;
    return `Reservation set for ${date} from ${timeStart} to ${timeEnd} for a total price of: ${totalPrice}`;
  }

  async onSubmit(): Promise<void> {
    const rentStart = new Date(this.selectedSlot.rentStart);
    const rentEnd = Utils.addMinutes(rentStart, this.duration);
    const reservation = {
      rentStart: rentStart.toISOString(),
      rentEnd: rentEnd.toISOString(),
      description: ""
    };
    const reservationStatus = await this.api.requestReservation(this.roomData.id, reservation);
    if (reservationStatus.status.success) {
      this.duration = 0;
      this.date = null;
      this.selectedSlot = null;
      this.slots = [];
      this.dateFormControl.setValue(new Date());
    }

    Utils.openSnackbar(this.snackBar, reservationStatus.status.success, reservationStatus.status.message || "Reservation sent.");
  }
}
