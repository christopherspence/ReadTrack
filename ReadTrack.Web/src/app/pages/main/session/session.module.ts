import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SessionListComponent } from './session-list';
import { AddEditSessionDialogComponent } from './add-edit-session-dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from '../../../shared';
import { MatInputModule } from '@angular/material/input';

const routes: Routes = [{ path: '', component: SessionListComponent}]

@NgModule({
    imports: [
        MatButtonModule,        
        MatDatepickerModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatNativeDateModule,
        MatSnackBarModule,
        NgxDatatableModule,
        RouterModule.forChild(routes),
        SharedModule
    ],
    declarations: [
        SessionListComponent,
        AddEditSessionDialogComponent
    ]
})
export class SessionModule { }