import { Component, Input, OnInit } from '@angular/core';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';

@Component({
  selector: 'app-room-view-header',
  templateUrl: './room-view-header.component.html',
  styleUrls: ['./room-view-header.component.scss']
})
export class RoomViewHeaderComponent implements OnInit {
  @Input() roomData: RoomData;

  constructor() { }

  ngOnInit(): void {
  }

  calcPicNumber(): number {
    const seed = `${this.roomData.name}${this.roomAddress()}`;
    return Utils.calcPicNumber(seed);
  }

  roomAddress(): string {
    return Utils.roomAddress(this.roomData);
  }

  getlocationLink(): string {
    return `https://www.google.com/maps?q=${this.roomData.lat},${this.roomData.lon}`;
  }
}
