import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../../../core/services'

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

    constructor(private fb: FormBuilder, private authService: AuthService) {
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
        } catch (e) {
            alert('An error occurred while logging in');            
        }
        
        this.loading = false;                    
    }
}