import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SessionListComponent } from './session-list';

const routes: Routes = [{ path: '', component: SessionListComponent}]

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    declarations: [
        SessionListComponent
    ]
})
export class SessionModule { }