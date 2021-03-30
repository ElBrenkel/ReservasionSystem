import { DOCUMENT } from '@angular/common';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { CalendarEvent, CalendarEventAction, CalendarView } from 'angular-calendar';
import { addDays, isSameDay, isSameMonth } from 'date-fns';
import { Utils } from '../common/utils';
import { Reservation } from '../interfaces/reservation';
import { ReservationSystemApiService } from '../reservation-system-api.service';

const colors: any = [
  { //yellow
    primary: '#e3bc08',
    secondary: '#343400',
  },
  { //green
    primary: '#32CD32',
    secondary: '#145314'
  },
  { //red
    primary: '#e32636',
    secondary: '#4c0a0f',
  },
];

@Component({
  selector: 'app-reservation-calendar',
  templateUrl: './reservation-calendar.component.html',
  styleUrls: ['./reservation-calendar.component.scss']
})
export class ReservationCalendarComponent implements OnInit {
  view: CalendarView = CalendarView.Month;
  @Input() roomId: number;
  reservations: Reservation[] = [];
  viewDate = new Date();

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fas fa-check ml10"></i>',
      a11yLabel: 'Approve',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent(event, "accept");
      },
    },
    {
      label: '<i class="fas fa-times ml10"></i>',
      a11yLabel: 'Reject',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent(event, "reject");
      },
    },
  ];

  async handleEvent(event: any, status: string) {
    event.actions = [];
    const response = status == "accept"
      ? await this.api.acceptReservation(this.roomId, event.reservation.id)
      : await this.api.rejectReservation(this.roomId, event.reservation.id);

    if (!response.success) {
      console.log(response.message);
    }

    await this.getReservations(false);
  }

  events: CalendarEvent[] = [];

  private readonly darkThemeClass = 'dark-theme';
  activeDayIsOpen: boolean = true;

  constructor(@Inject(DOCUMENT) private document, private api: ReservationSystemApiService) { }

  ngOnInit(): void {
    this.document.body.classList.add(this.darkThemeClass);
    this.getReservations(true);
  }

  async getReservations(init: boolean) {
    const response = await this.api.getReservations(this.roomId);
    if (response.status.success) {
      this.reservations = response.items;
      this.createEvents();
      const todayEvents = this.events.filter(x => isSameDay(x.start, this.viewDate));
      if (init && (!todayEvents || todayEvents.length == 0)) {
        this.activeDayIsOpen = false;
      }
    }
    else {
      console.log(response.status.message);
      this.reservations = [];
    }
  }

  createEvents() {
    this.events = [];
    for (const reservation of this.reservations) {
      const event = {
        title: this.getEventTitle(reservation),
        start: new Date(reservation.rentStart),
        end: new Date(reservation.rentEnd),
        color: this.getEventColor(reservation),
        actions: reservation.status === 1 ? this.actions : [],
        reservation: reservation
      }

      this.events.push(event);
    }
  }
  getEventTitle(reservation: Reservation) {
    return `${reservation.userFullName}: ${Utils.getHourFromDate(new Date(reservation.rentStart))} - ${Utils.getHourFromDate(new Date(reservation.rentEnd))}`
  }
  getEventColor(reservation: Reservation) {
    return colors[(reservation.status || 1) - 1];
  }

  ngOnDestroy(): void {
    this.document.body.classList.remove(this.darkThemeClass);
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }
}
