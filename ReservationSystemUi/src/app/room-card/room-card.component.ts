import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Utils } from '../common/utils';

@Component({
  selector: 'app-room-card',
  templateUrl: './room-card.component.html',
  styleUrls: ['./room-card.component.scss']
})
export class RoomCardComponent implements OnInit {
  @Input() roomId: number;
  @Input() roomName = "";
  @Input() roomAddress = "";
  @Input() isActive = true;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  calcPicNumber(): number {
    const seed = `${this.roomName}${this.roomAddress}`;
    return Utils.calcPicNumber(seed);
  }

  onCardClick(): void {
    if (this.isActive && this.roomId) {
      this.router.navigate([`room/${this.roomId}`]);
    }
  }
}
