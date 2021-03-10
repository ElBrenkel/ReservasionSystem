import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomEditWorkingHoursComponent } from './room-edit-working-hours.component';

describe('RoomEditWorkingHoursComponent', () => {
  let component: RoomEditWorkingHoursComponent;
  let fixture: ComponentFixture<RoomEditWorkingHoursComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoomEditWorkingHoursComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomEditWorkingHoursComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
