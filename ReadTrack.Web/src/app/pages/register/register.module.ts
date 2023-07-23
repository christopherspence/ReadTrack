import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RegisterComponent } from '.';
import { SharedModule } from '../../shared';


const routes = [{ path: '', component: RegisterComponent }];

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
        RegisterComponent        
    ]   
})
export class RegisterModule { }
