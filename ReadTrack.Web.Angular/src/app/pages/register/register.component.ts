import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmValidator, CreateUserRequest, SimpleDialogComponent } from '../../shared';
import { UserService } from '../../core/services';


@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
    form!: UntypedFormGroup;
    loading = false;

    get request() {
        const form = this.form?.value;

        return new CreateUserRequest(form.firstName, form.lastName, form.email, form.password);
    }

    get email() {
        return this.form.get('email');
    }

    get password() {
        return this.form.get('password');
    }

    get confirmPassword() {
        return this.form.get('confirmPassword');
    }

    constructor(
        private fb: UntypedFormBuilder, 
        private userService: UserService,
        private router: Router,
        public dialog: MatDialog) {

        this.setupForm();
    }

    private setupForm() {
        this.form = this.fb.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', Validators.required],
            password: ['', Validators.required],
            confirmPassword: ['', Validators.required]
        }, {
            validator: ConfirmValidator('password', 'confirmPassword')
        });
    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form.controls[controlName].hasError(errorName);
    }

    public async register() {
        this.loading = true;

        try {
            await this.userService.register(this.request).toPromise();

            this.router.navigate(['']);
        } catch (e: any) {
            this.dialog.open(SimpleDialogComponent, {
                width: '250px',
                data: {
                    title: 'Error',
                    message: e.error?.error
                }
            });
        }
        
        this.loading = false;                    
    }
}