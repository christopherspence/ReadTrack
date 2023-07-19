import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookListComponent } from './book-list/book-list.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { SharedModule } from '../../../shared';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AddEditBookDialogComponent } from './add-edit-book-dialog-component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';

const routes: Routes = [{ path: '', component: BookListComponent}];

@NgModule({
    imports: [
        MatButtonModule,        
        MatCheckboxModule,
        MatDatepickerModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatNativeDateModule,
        MatSelectModule,
        MatSnackBarModule,
        NgxDatatableModule,
        RouterModule.forChild(routes),
        SharedModule],
    declarations: [
        AddEditBookDialogComponent,
        BookListComponent
    ]
})
export class BookModule { }