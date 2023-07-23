import { NgModule } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { ProfileComponent } from './profile.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SharedModule } from '../../../shared';

const routes = [{ path: '', component: ProfileComponent }];

@NgModule({
    imports: [      
        RouterModule.forChild(routes),  
        MatCardModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatSnackBarModule,
        SharedModule        
    ],
    declarations: [
        ProfileComponent
    ]
})
export class ProfileModule { }