import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reservation-status-snackbar',
  templateUrl: './reservation-status-snackbar.component.html',
  styleUrls: ['./reservation-status-snackbar.component.scss']
})
export class ReservationStatusSnackbarComponent implements OnInit {

  constructor(
    public snackBarRef: MatSnackBarRef<ReservationStatusSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: any
  ) { }

  ngOnInit(): void {
  }

  getIcon(): string {
    return this.data.success ? "done" : "clear";
  }

  getMessage(): string {
    return this.data.message;
  }
}
