import { ValidatorFn, AbstractControl } from '@angular/forms';
import { isValidNumber } from 'libphonenumber-js';

export default function PhoneNumberValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    let validNumber = false;
    try {
      validNumber = isValidNumber(control.value);
    } catch (e) {}

    return validNumber ? null : { wrongNumber: { value: control.value } };
  };
}
