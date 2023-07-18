import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SearchBoxComponent } from './components/search-box';

const modules = [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
];


@NgModule({
    imports: [
        ...modules
    ],
    exports: [
        ...modules,
        SearchBoxComponent
    ],
    declarations: [
        SearchBoxComponent
    ]
})
export class SharedModule { }