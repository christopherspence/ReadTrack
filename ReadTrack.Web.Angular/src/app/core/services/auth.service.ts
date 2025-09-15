import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoginRequest, TokenResponse } from 'src/app/shared';
import { UserService } from './user.service';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(
        private http: HttpClient,
        private userService: UserService) { }

    login(email: string, password: string): Observable<object> {
        return this.http.post('auth/login', new LoginRequest(email, password))
            .pipe(tap(res => {
                const tokenResponse = res as TokenResponse;
                this.userService.setTokenInfo(tokenResponse)
                this.userService.setUserInfo(tokenResponse.user);
            }));
    }
}