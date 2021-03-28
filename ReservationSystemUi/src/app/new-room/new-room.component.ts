import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { RoomData } from '../interfaces/roomData';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-new-room',
  templateUrl: './new-room.component.html',
  styleUrls: ['./new-room.component.scss']
})
export class NewRoomComponent implements OnInit {
  roomData: RoomData = {
    id: 0,
    isActive: true,
    isOwner: true,
    workingHours: []
  };

  loading = false;
  editMode = true;

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  roomActiveOrOwnedByUser(): boolean {
    return this.roomData.isActive || this.roomData.isOwner;
  }

  ngOnInit(): void {
  }

  goToHome() {
    this.router.navigate([""]);
  }

  editModeChanged(e: any): void {
    this.editMode = e;
  }
}
