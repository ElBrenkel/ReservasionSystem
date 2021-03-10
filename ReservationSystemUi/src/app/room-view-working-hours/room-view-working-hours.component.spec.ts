import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomViewWorkingHoursComponent } from './room-view-working-hours.component';

describe('RoomViewWorkingHoursComponent', () => {
  let component: RoomViewWorkingHoursComponent;
  let fixture: ComponentFixture<RoomViewWorkingHoursComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoomViewWorkingHoursComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomViewWorkingHoursComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
