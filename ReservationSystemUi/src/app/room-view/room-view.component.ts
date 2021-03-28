import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { RoomData } from '../interfaces/roomData';
import { ReservationSystemApiService } from '../reservation-system-api.service';
import { last } from "lodash";

@Component({
  selector: 'app-room-view',
  templateUrl: './room-view.component.html',
  styleUrls: ['./room-view.component.scss']
})
export class RoomViewComponent implements OnInit {
  roomData: RoomData = null;
  loading = false;
  routerEventsSubscription: Subscription;
  editMode = false;

  constructor(private api: ReservationSystemApiService, private router: Router) {
    this.routerEventsSubscription = this.router.events.subscribe(x => {
      if (x instanceof NavigationEnd && !this.loading) {
        this.loading = true;
        this.roomData = null;
        this.loadData(x.url);
      }
    });
  }

  async loadData(url: string): Promise<void> {
    const id = last(url.split("/"));
    try {
      const response = await this.api.getRoom(id);
      if (response.status.success) {
        this.roomData = response.object;
      }
    }
    catch (e) {
      console.log({ e });
    }
    this.loading = false;
    if (!this.roomData || !this.roomActiveOrOwnedByUser()) {
      this.goToHome();
    }
  }

  roomActiveOrOwnedByUser(): boolean {
    return this.roomData.isActive || this.roomData.isOwner;
  }

  ngOnDestroy(): void {
    this.routerEventsSubscription.unsubscribe();
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
