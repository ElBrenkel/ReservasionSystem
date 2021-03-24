import { Component, Input, OnInit } from '@angular/core';
import { Utils } from '../common/utils';
import { WorkingHours } from '../interfaces/workingHours';

@Component({
  selector: 'app-room-edit-working-hours',
  templateUrl: './room-edit-working-hours.component.html',
  styleUrls: ['./room-edit-working-hours.component.scss']
})
export class RoomEditWorkingHoursComponent implements OnInit {
  @Input() workingHours: WorkingHours[];
  sortedWorkingHours: any;
  activeDays: string[] = [];
  days = [];

  constructor() { }

  ngOnInit(): void {
    this.days = Object.keys(Utils.dayStrings).map(x => {
      return {
        key: x,
        name: Utils.dayStrings[x],
        char: Utils.dayStrings[x].charAt(0),
        active: false
      };
    });
    this.constructSortedDays();
  }

  getDayString(day: number): string {
    return Utils.getDayString(day);
  }

  getHour(time: number): string {
    return Utils.getHour(time);
  }

  onDayClicked(day: any): void {
    if (day.active) {
      this.workingHours = this.workingHours.filter(x => x.day != day.key);
    }
    else {
      const defaultWorkingHour: WorkingHours = {
        day: day.key * 1,
        timeStart: 540,
        timeEnd: 1080,
        priceForHour: 50
      };
      this.workingHours.push(defaultWorkingHour);
    }

    this.constructSortedDays();
  }

  constructSortedDays(): void {
    this.sortedWorkingHours = {};
    for (const workingHour of this.workingHours) {
      if (!this.sortedWorkingHours[workingHour.day]) {
        this.sortedWorkingHours[workingHour.day] = [];
      }
      this.sortedWorkingHours[workingHour.day].push(workingHour);
    }

    for (const day of Object.keys(this.sortedWorkingHours)) {
      this.sortedWorkingHours[day].sort((x, y) => x.timeStart - y.timeStart);
    }

    this.activeDays = Object.keys(this.sortedWorkingHours);
    for (const dayObj of this.days) {
      dayObj.active = false;
    }

    for (const day of this.activeDays) {
      const dayObj = this.days.find(x => x.key == day);
      if (dayObj) {
        dayObj.active = true;
      }
    }
  }
}
