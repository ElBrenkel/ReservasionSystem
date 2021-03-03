import { Component, EventEmitter, OnChanges, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { debounce } from "lodash";

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {
  @Output() searchValueChangedEvent = new EventEmitter();
  value: string;
  private debouncedEmitSearch = debounce(() => this.searchValueChangedEvent.emit(this.value), 250);

  constructor() { }

  ngOnInit(): void {
  }

  onSearchChange(e) {
    this.value = e;
    this.debouncedEmitSearch();
  }
}
