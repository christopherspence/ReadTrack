import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmValidator, CreateUserRequest, SimpleDialogComponent } from '../../shared';
import { UserService } from '../../core/services';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';


@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    standalone: true,
    imports: [
        FormsModule, 
        MatCardModule, 
        MatDialogModule, 
        MatFormFieldModule, 
        MatInputModule,         
        MatButtonModule,
        NgIf, 
        ReactiveFormsModule
    ]
})
export class RegisterComponent {
    form?: UntypedFormGroup;
    loading = false;

    constructor(
        private fb: UntypedFormBuilder, 
        private userService: UserService,
        private router: Router,
        public dialog: MatDialog) {

        this.setupForm();
    }

    get request() {
        const form = this.form?.value;

        return new CreateUserRequest(form?.firstName, form?.lastName, form?.email, form?.password);
    }

    get email() {
        return this.form?.get('email');
    }

    get password() {
        return this.form?.get('password');
    }

    get confirmPassword() {
        return this.form?.get('confirmPassword');
    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form?.controls[controlName].hasError(errorName);
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
}