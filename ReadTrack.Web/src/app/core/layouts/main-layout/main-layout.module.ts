import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MainLayoutRoutingModule } from './main-layout-routing.module';
import { MainLayoutComponent } from './main-layout.component';

@NgModule({
    imports: [
        MainLayoutRoutingModule,
        MatSidenavModule,
        MatIconModule,
        MatButtonModule
    ],
    declarations: [MainLayoutComponent]
})
export class MainLayoutModule { }
