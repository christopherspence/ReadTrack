import { FormGroup } from '@angular/forms';

export function ConfirmValidator(controlName: string, confirmControlName: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[confirmControlName];

        if (matchingControl.errors && !matchingControl.errors['confirmedValidator']) {
            return;
        }

        if (control.value !== matchingControl.value) {
            matchingControl.setErrors({ confirmedValidator: true })
        } else {
            matchingControl.setErrors(null);
        }
    };
}