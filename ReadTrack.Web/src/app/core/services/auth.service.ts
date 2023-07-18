import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { LOCAL_STORAGE, StorageService } from 'ngx-webstorage-service';
import { Observable, tap } from 'rxjs';
import { LoginRequest, TokenResponse } from 'src/app/shared';

const USER_ID = 'user_id';
const USER_EMAIL = 'user_email';
const USER_TOKEN = 'id_token';
const EXPIRES_AT = 'expires_at';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(
        private http: HttpClient,
        @Inject(LOCAL_STORAGE) private storage: StorageService) { }

    login(email: string, password: string): Observable<object> {
        return this.http.post('auth/login', new LoginRequest(email, password))
            .pipe(tap(res => {
                this.setUserInfo(res as TokenResponse);
            }));
    }

    getToken(): string {
        return this.storage.get(USER_TOKEN) ?? '';
    }

    private setUserInfo(res: TokenResponse): void {
        this.storage.set(USER_TOKEN, res.token) ;
        this.storage.set(USER_ID, res.user.id.toString());
        this.storage.set(USER_EMAIL, res.user.email);
        this.storage.set(EXPIRES_AT, res.expires.toString());
    }

    logout(): void {
        this.storage.remove(USER_TOKEN);
        this.storage.remove(USER_ID);
        this.storage.remove(USER_EMAIL);
        this.storage.remove(EXPIRES_AT);
    }

    isLoggedIn(): boolean {
        return !!this.storage.get(USER_TOKEN);
    }

    isLoggedOut(): boolean {
        return !this.isLoggedIn();
    }
}