import { Component, Input, OnInit } from '@angular/core';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';
import { WorkingHours } from '../interfaces/workingHours';

@Component({
  selector: 'app-room-view-working-hours',
  templateUrl: './room-view-working-hours.component.html',
  styleUrls: ['./room-view-working-hours.component.scss']
})
export class RoomViewWorkingHoursComponent implements OnInit {
  @Input() roomData: RoomData;
  sortedWorkingHours: any;
  activeDays: string[] = [];

  constructor() { }

  ngOnInit(): void {
    this.sortedWorkingHours = {};
    for (const workingHour of this.roomData.workingHours) {
      if (!this.sortedWorkingHours[workingHour.day]) {
        this.sortedWorkingHours[workingHour.day] = [];
      }
      this.sortedWorkingHours[workingHour.day].push(workingHour);
    }
    for (const day of Object.keys(this.sortedWorkingHours)) {
      this.sortedWorkingHours[day].sort((x, y) => x.timeStart - y.timeStart);
    }
    this.activeDays = Object.keys(this.sortedWorkingHours);
  }

  getDayString(day: number): string {
    return Utils.getDayString(day);
  }

  getHour(time: number): string {
    return Utils.getHour(time);
  }
}
