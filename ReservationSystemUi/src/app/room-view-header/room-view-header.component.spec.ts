import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomViewHeaderComponent } from './room-view-header.component';

describe('RoomViewHeaderComponent', () => {
  let component: RoomViewHeaderComponent;
  let fixture: ComponentFixture<RoomViewHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoomViewHeaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomViewHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
