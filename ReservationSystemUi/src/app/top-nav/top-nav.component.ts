import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { debounce } from "lodash";

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {
  @Output() onSearchClickedEvent = new EventEmitter();
  @Output() searchValueChangedEvent = new EventEmitter();
  value: string;

  private debouncedEmitSearch = debounce(() => this.searchValueChangedEvent.emit(this.value), 250);

  constructor() { }

  ngOnInit(): void {
    this.value = sessionStorage["searchValue"] || "";
    if (this.value) {
      this.onSearchChange(this.value);
    }
  }

  onSearchChange(e) {
    this.value = e;
    sessionStorage["searchValue"] = this.value;
    this.debouncedEmitSearch();
  }

  onSearchClicked(): void {
    if (this.onSearchClickedEvent) {
      this.onSearchClickedEvent.emit();
    }
  }
}
