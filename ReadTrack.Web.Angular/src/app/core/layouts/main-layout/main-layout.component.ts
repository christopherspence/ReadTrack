import { Component } from '@angular/core';
import { UserService } from '../../services';
import { Router } from '@angular/router';

@Component({
    selector: 'app-main-layout',
    standalone: false,
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent {
    isMenuOpen = false;

    constructor(
        private router: Router,
        private userService: UserService) {

    }

    sideNavClicked(): void {
        this.isMenuOpen = false;
    }

    logout(): void {
        this.userService.logout();

        this.router.navigate(['login']);
    }
}