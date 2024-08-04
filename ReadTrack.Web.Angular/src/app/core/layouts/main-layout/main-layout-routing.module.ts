import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        loadChildren: () => import('../../../pages/main/dashboard/dashboard.module').then(m => m.DashboardModule)
    },
    {
        path: 'books',
        loadChildren: () => import('../../../pages/main/book/book.module').then(m => m.BookModule)
    },
    {
        path: 'sessions/:id',
        loadChildren: () => import('../../../pages/main/session/session.module').then(m => m.SessionModule)
    },
    {
        path: 'profile',
        loadChildren: () => import('../../../pages/main/profile/profile.module').then(m => m.ProfileModule)
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MainLayoutRoutingModule { }
