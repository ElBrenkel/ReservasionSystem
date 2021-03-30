import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationCalendarHeaderComponent } from './reservation-calendar-header.component';

describe('ReservationCalendarHeaderComponent', () => {
  let component: ReservationCalendarHeaderComponent;
  let fixture: ComponentFixture<ReservationCalendarHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReservationCalendarHeaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReservationCalendarHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
