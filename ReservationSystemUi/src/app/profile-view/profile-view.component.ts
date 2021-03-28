import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../interfaces/user';
import { ReservationSystemApiService } from '../reservation-system-api.service';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.scss']
})
export class ProfileViewComponent implements OnInit {
  user: User;

  constructor(private api: ReservationSystemApiService, private router: Router) { }

  ngOnInit(): void {
    this.getUserData();
  }

  async getUserData(): Promise<void> {
    const response = await this.api.getUserData();
    if (response.status.success) {
      this.user = response.object;
    }
    else {
      this.goToHome();
    }
  }

  goToHome() {
    this.router.navigate([""]);
  }
}
