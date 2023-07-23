import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { LoginComponent } from '.';
import { SharedModule } from '../../shared';

const routes = [{ path: '', component: LoginComponent }];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        MatCardModule,
        MatFormFieldModule,
        MatButtonModule,
        MatInputModule,
        SharedModule
    ],
    declarations: [
        LoginComponent        
    ]   
})
export class AuthModule { }
