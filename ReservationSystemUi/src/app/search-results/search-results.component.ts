import { Component, Input, OnInit } from '@angular/core';
import { utils } from 'protractor';
import { Utils } from '../common/utils';
import { RoomData } from '../interfaces/roomData';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.scss']
})
export class SearchResultsComponent implements OnInit {
  @Input() rooms: RoomData[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  roomAddress(room: RoomData): string {
    return Utils.roomAddress(room);
  }
}
