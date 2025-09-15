import { Routes } from '@angular/router';
import { AuthGuard } from './core';
import { MainLayoutComponent } from './core/layouts';
import { DashboardComponent } from './pages/main/dashboard';
import { BookListComponent } from './pages/main/book/book-list';
import { SessionListComponent } from './pages/main/session/session-list';
import { ProfileComponent } from './pages/main/profile';
import { LoginComponent } from './pages/login';
import { RegisterComponent } from './pages/register';

export const routes: Routes = [{
    path: '',
    component: MainLayoutComponent,
    children:[
        {
            path: '',
            redirectTo: 'dashboard',
            pathMatch: 'full'
        },
        {
            path: 'dashboard',
            component: DashboardComponent
        },
        {
            path: 'books',
            component: BookListComponent
        },
        {
            path: 'sessions/:id',
            component: SessionListComponent
        },
        {
            path: 'profile',
            component: ProfileComponent
        }
    ],
    canActivate: [AuthGuard]
}, {
    path: 'login',
    component: LoginComponent
}, {
    path: 'register',
    component: RegisterComponent
}];

