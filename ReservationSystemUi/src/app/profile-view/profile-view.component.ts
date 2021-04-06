import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Reservation } from '../interfaces/reservation';
import { RoomData } from '../interfaces/roomData';
import { User } from '../interfaces/user';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.scss']
})
export class ProfileViewComponent implements OnInit {
  user: User;
  rooms: RoomData[] = [];
  isOwner: boolean = false;

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
    this.getUserData();
  }

  async getUserData(): Promise<void> {
    const response = await this.api.getUserData();
    if (response.status.success) {
      this.user = response.object;
      await this.getUserRooms();
    }
    else {
      this.goToHome();
    }
  }

  async getUserRooms(): Promise<void> {
    const response = await this.api.getOwnedRooms();
    if (response.status.success) {
      this.rooms = response.items;
      this.isOwner = true;
    }
    else {
      console.log(response.status.message);
      this.isOwner = false;
      this.rooms = [];
    }
  }

  addNewRoom(): void {
    document.body.scrollTop = document.documentElement.scrollTop = 0;
    this.router.navigate(["room"]);
  }

  goToHome() {
    this.router.navigate([""]);
  }
}
