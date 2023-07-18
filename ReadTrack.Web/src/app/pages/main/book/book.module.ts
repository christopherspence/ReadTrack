import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookListComponent } from './book-list/book-list.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SharedModule } from '../../../shared';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

const routes: Routes = [{ path: '', component: BookListComponent}];

@NgModule({
    imports: [
        MatButtonModule,
        MatCheckboxModule,
        MatFormFieldModule,
        NgxDatatableModule,
        RouterModule.forChild(routes),
        SharedModule],
    declarations: [
        BookListComponent
    ]
})
export class BookModule { }