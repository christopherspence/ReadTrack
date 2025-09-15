import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmValidator, SimpleDialogComponent, StringUtilities, User } from '../../../shared';
import { UserService } from '../../../core/services';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css'],
    standalone: true,
    imports: [
        FormsModule, 
        MatButtonModule, 
        MatCardModule, 
        MatDialogModule, 
        MatFormFieldModule, 
        MatInputModule, 
        MatSnackBarModule, 
        NgIf, 
        ReactiveFormsModule
    ]
})

export class ProfileComponent implements OnInit {
    form?: UntypedFormGroup;
    loading = false;
    imageSource = '';
    imageChanged = false;
    passowrdChanged = false;

    user?: User;

    constructor(
        public sanitizer: DomSanitizer,
        private fb: UntypedFormBuilder, 
        private userService: UserService,
        public dialog: MatDialog,
        private snackBar: MatSnackBar) {

        this.setupForm();
    }

    async ngOnInit(): Promise<void> {
        await this.getUserInfo();
    }

    async getUserInfo(): Promise<void> {
        this.user = await this.userService.getUser().toPromise();

        if (this.user) {
            this.form?.patchValue({
                firstName: this.user.firstName,
                lastName: this.user.lastName,
                email: this.user.email
            });

            if (this.user.profilePicture) {
                this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/jpg;base64,${this.user.profilePicture}`) as string; 
            } else {
                this.imageSource = './assets/img/profile.png';
            }
        }
    }

    private setupForm() {
        this.form = this.fb.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', Validators.required],
            password: [''],
            confirmPassword: ['']
        }, {
            validator: ConfirmValidator('password', 'confirmPassword')
        });
    }

    public hasError = (controlName: string, errorName: string) => {
        return this.form?.controls[controlName].hasError(errorName);
    }

    onFileChange(event: Event): void {
        const reader = new FileReader();
        const files = (event.target as HTMLInputElement).files;

        if (files && files.length) {
            reader.readAsDataURL(files[0]);

            reader.onload = () => {
                this.imageSource = reader.result as string;
                this.imageChanged = true;
            };
        }
    }

    public async submit() {
        if (this.user) {
            this.loading = true;

            const form = this.form?.value;
            
            this.user.firstName = form?.firstName;
            this.user.lastName = form?.lastName;
            // TODO: disabling this for now until I can figure out the token issue after changing email
            // this.user.email = form?.email;
            
            if (this.imageChanged) {
                this.user.profilePicture = StringUtilities.removeBase64Prefix(this.imageSource);
            }
            if (this.passowrdChanged) {
                this.user.password = form?.password;
            }

            try {
                await this.userService.updateUser(this.user).toPromise();
    
                this.snackBar.open('Profile updated successfully', '',  {
                    duration: 2000,
                    verticalPosition: 'top',
                    horizontalPosition: 'right',
                });
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
}