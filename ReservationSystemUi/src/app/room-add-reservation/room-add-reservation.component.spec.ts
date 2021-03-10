import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomAddReservationComponent } from './room-add-reservation.component';

describe('RoomAddReservationComponent', () => {
  let component: RoomAddReservationComponent;
  let fixture: ComponentFixture<RoomAddReservationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoomAddReservationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomAddReservationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
