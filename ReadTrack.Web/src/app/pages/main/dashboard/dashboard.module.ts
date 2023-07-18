import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';

const routes = [{ path: '', component: DashboardComponent }];

@NgModule({
    imports: [RouterModule.forChild(routes)]
})
export class DashboardModule { }