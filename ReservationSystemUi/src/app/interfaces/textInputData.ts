import { FormControl } from "@angular/forms";

export interface TextInputData {
    inputType: string;
    label: string;
    prop: string;
    control: FormControl;
    placeholder: string;
}