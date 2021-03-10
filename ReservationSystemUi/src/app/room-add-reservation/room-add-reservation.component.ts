import { Component, Input, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { promise } from 'selenium-webdriver';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';
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

  workingHoursFilter = (d: Date | null): boolean => {
    const workingHoursDays = this.roomData.workingHours.map(x => x.day);
    const day = (d || new Date()).getDay() + 1;
    return workingHoursDays.includes(day);
  }

  constructor(private api: ReservationSystemApiService) { }

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
}
