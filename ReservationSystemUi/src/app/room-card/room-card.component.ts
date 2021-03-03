import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-room-card',
  templateUrl: './room-card.component.html',
  styleUrls: ['./room-card.component.scss']
})
export class RoomCardComponent implements OnInit {
  @Input() roomName = "";
  @Input() roomAddress = "";
  @Input() isActive = true;

  constructor() { }

  ngOnInit(): void {
  }

  calcPicNumber(): number {
    const seed = `${this.roomName}${this.roomAddress}`;
    const number = seed.split("").map(x => x.charCodeAt(0)).reduce((x, y) => x + y);
    return (number % 6) + 1;
  }
}
