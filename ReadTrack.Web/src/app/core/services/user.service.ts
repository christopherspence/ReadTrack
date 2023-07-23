import { Inject, Injectable } from '@angular/core';
import { BaseHttpService } from './base-http-service';
import { HttpClient } from '@angular/common/http';
import { CreateUserRequest, TokenResponse } from '../../shared';
import { Observable, tap } from 'rxjs';
import { LOCAL_STORAGE, StorageService } from 'ngx-webstorage-service';

const USER_ID = 'user_id';
const USER_EMAIL = 'user_email';
const USER_TOKEN = 'id_token';
const EXPIRES_AT = 'expires_at';

@Injectable({
    providedIn: 'root'
})
export class UserService extends BaseHttpService {
    constructor(
        http: HttpClient,
        @Inject(LOCAL_STORAGE) private storage: StorageService) { super(http); }

    register(request: CreateUserRequest): Observable<object> {
        return this.http.post('user/register', request)
            .pipe(tap(res => {
                this.setUserInfo(res as TokenResponse);
            }));
    }

    setUserInfo(res: TokenResponse): void {
        this.storage.set(USER_TOKEN, res.token) ;
        this.storage.set(USER_ID, res.user.id.toString());
        this.storage.set(USER_EMAIL, res.user.email);
        this.storage.set(EXPIRES_AT, res.expires.toString());
    }

    getToken(): string {
        return this.storage.get(USER_TOKEN) ?? '';
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