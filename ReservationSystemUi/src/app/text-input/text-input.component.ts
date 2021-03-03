import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MyErrorStateMatcher } from '../common/my-error-state-matcher';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit {

  @Input() inputType = "text";
  @Input() label = "label";
  @Input() model: any;
  @Input() prop = "";
  @Input() control: FormControl;
  @Input() placeholder = "";

  currentInputType = "";
  matcher = new MyErrorStateMatcher();
  rightIcon = "visibility";

  hasErrors(): boolean {
    return this.control.errors !== null;
  }

  getFirstError(): string {
    if (this.control.errors === null) {
      return null;
    }
    else if (this.control.errors.required) {
      return `${this.label} is required.`;
    }
    else if (this.control.errors.email) {
      return "Please enter a valid email address.";
    }
    else if (this.control.errors.maxlength) {
      const length = this.control.errors.maxlength.requiredLength;
      return `${this.label} is restricted to ${length} characters.`;
    }
    else if (this.control.errors.minlength) {
      const length = this.control.errors.minlength.requiredLength;
      return `${this.label} should be at least ${length} characters.`;
    }
    else if (this.control.errors.min) {
      const min = this.control.errors.min.min;
      return `${this.label} should be at least ${min}.`;
    }
    return null;
  }

  constructor() { }

  ngOnInit(): void {
    this.currentInputType = this.inputType;
  }

  togglePasswordVisibility(showPassword: boolean): void {
    if (this.inputType === "password") {
      this.rightIcon = showPassword ? "visibility_off" : "visibility";
      this.currentInputType = showPassword ? "text" : "password";
    }
  }
}
