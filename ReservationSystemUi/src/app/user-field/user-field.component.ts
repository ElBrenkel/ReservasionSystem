import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-user-field',
  templateUrl: './user-field.component.html',
  styleUrls: ['./user-field.component.scss']
})
export class UserFieldComponent implements OnInit {
  @Input() user: any;
  @Input() prop: string;
  @Input() label: string;
  @Input() type: string = "text";
  @Output() valueChanged = new EventEmitter();
  editMode: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  onFieldClicked(): void {
    if (!this.editMode) {
      this.editMode = true;
    }
  }

  onValueChanged(): void {
    this.valueChanged.emit(this.prop);
    this.editMode = false;
  }
}
