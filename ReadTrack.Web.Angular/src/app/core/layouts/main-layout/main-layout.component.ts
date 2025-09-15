import { Component } from '@angular/core';
import { UserService } from '../../services';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.css'],
    standalone: true,
    imports: [
        MatButtonModule,
         MatIconModule,          
         MatSidenavModule,          
         RouterLink, 
         RouterOutlet
    ]
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