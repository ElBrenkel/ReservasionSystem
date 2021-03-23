import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationStatusSnackbarComponent } from './reservation-status-snackbar.component';

describe('ReservationStatusSnackbarComponent', () => {
  let component: ReservationStatusSnackbarComponent;
  let fixture: ComponentFixture<ReservationStatusSnackbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReservationStatusSnackbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReservationStatusSnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
