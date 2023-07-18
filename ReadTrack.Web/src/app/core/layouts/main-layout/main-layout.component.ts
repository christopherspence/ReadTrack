import { Component } from '@angular/core';
import { AuthService } from '../../services';
import { Router } from '@angular/router';

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent {
    isMenuOpen = false;

    constructor(
        private router: Router,
        private authService: AuthService) {

    }

    sideNavClicked(): void {
        this.isMenuOpen = false;
    }

    logout(): void {
        this.authService.logout();

        this.router.navigate(['login']);
    }
}