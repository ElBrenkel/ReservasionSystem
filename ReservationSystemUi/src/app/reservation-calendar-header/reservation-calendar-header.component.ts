import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CalendarView } from 'angular-calendar';

@Component({
  selector: 'app-reservation-calendar-header',
  templateUrl: './reservation-calendar-header.component.html',
  styleUrls: ['./reservation-calendar-header.component.scss']
})
export class ReservationCalendarHeaderComponent implements OnInit {
  @Input() view: CalendarView;

  @Input() viewDate: Date;

  @Input() locale: string = 'en';

  @Output() viewChange = new EventEmitter<CalendarView>();

  @Output() viewDateChange = new EventEmitter<Date>();

  CalendarView = CalendarView;
  constructor() { }

  ngOnInit(): void {
  }

}
