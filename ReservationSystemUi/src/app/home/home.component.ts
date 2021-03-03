import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GenericList } from '../interfaces/genericListResponse';
import { RoomData } from '../interfaces/roomData';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  rooms: RoomData[];

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
  }

  async searchByCityOrName(e) {
    try {
      const response: GenericList<RoomData> = await this.api.searchRooms(e);
      if (response.status.success) {
        this.rooms = response.items;
      }
    }
    catch (e) {
      console.log(e);
    }
  }
}
