import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services'
import { SimpleDialogComponent } from '../../shared';


@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    form: FormGroup;
    loading = false;

    get email() {
        return this.form.get('email');
    }

    get password() {
        return this.form.get('password');
    }

    constructor(
        private fb: FormBuilder, 
        private authService: AuthService,
        private router: Router,
        public dialog: MatDialog) {
        this.form = fb.group({
            email: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form.controls[controlName].hasError(errorName);
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