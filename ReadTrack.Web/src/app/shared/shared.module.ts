import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmDialogComponent, SearchBoxComponent, SimpleDialogComponent } from './components';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

const modules = [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
];


@NgModule({
    imports: [
        ...modules,
        MatButtonModule,
        MatDialogModule
    ],
    exports: [
        ...modules,
        ConfirmDialogComponent,
        SearchBoxComponent,
        SimpleDialogComponent
    ],
    declarations: [
        ConfirmDialogComponent,
        SearchBoxComponent,
        SimpleDialogComponent
    ]
})
export class SharedModule { }