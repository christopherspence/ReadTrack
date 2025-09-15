import { Component } from '@angular/core';
import { UntypedFormBuilder, Validators, UntypedFormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services'
import { SimpleDialogComponent } from '../../shared';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';


@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
    imports: [
        FormsModule, 
        MatButtonModule,
        MatCardModule, 
        MatDialogModule, 
        MatFormFieldModule, 
        MatInputModule, 
        NgIf, 
        ReactiveFormsModule, 
        RouterLink
    ]
})
export class LoginComponent {
    form?: UntypedFormGroup;
    loading = false;

    constructor(
        fb: UntypedFormBuilder, 
        private authService: AuthService,
        private router: Router,
        public dialog: MatDialog) {
        this.form = fb.group({
            email: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    get email() {
        return this.form?.get('email');
    }

    get password() {
        return this.form?.get('password');
    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form?.controls[controlName].hasError(errorName);
    }

    public async login() {
        this.loading = true;

        try {
            await this.authService.login(this.email?.value, this.password?.value).toPromise();

            this.router.navigate(['']);
        } catch (e) {
            this.dialog.open(SimpleDialogComponent, {
                width: '250px',
                data: {
                    title: 'Error',
                    message: 'An error occurred while logging in'
                }
            });
        }
        
        this.loading = false;                    
    }
}