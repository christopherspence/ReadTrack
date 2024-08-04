import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core';
import { MainLayoutComponent } from './core/layouts';

const routes: Routes = [{
    path: '',
    component: MainLayoutComponent,
    loadChildren: () => import('./core/layouts/main-layout/main-layout.module').then(m => m.MainLayoutModule),
    canActivate: [AuthGuard]
}, {
    path: 'login',
    loadChildren: () => import('./pages/login/login.module').then(m => m.AuthModule)
}, {
    path: 'register',
    loadChildren: () => import('./pages/register/register.module').then(m => m.RegisterModule)
}];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
