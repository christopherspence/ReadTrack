import { Inject, Injectable } from '@angular/core';
import { BaseHttpService } from './base-http-service';
import { HttpClient } from '@angular/common/http';
import { LOCAL_STORAGE, StorageService } from 'ngx-webstorage-service';
import { Observable, tap } from 'rxjs';
import { CreateUserRequest, TokenResponse, User } from '../../shared';

const USER_ID = 'user_id';
const USER_EMAIL = 'user_email';
const USER_TOKEN = 'id_token';
const EXPIRES_AT = 'expires_at';

const USER_URL = 'user';

@Injectable({
    providedIn: 'root'
})
export class UserService extends BaseHttpService {
    constructor(
        http: HttpClient,
        @Inject(LOCAL_STORAGE) private storage: StorageService) { super(http); }

    getUser(): Observable<User> {
        return this.http.get<User>(`${USER_URL}`);
    }

    register(request: CreateUserRequest): Observable<object> {
        return this.http.post(`${USER_URL}/register`, request)
            .pipe(tap(res => {
                this.setTokenInfo(res as TokenResponse);
            }));
    }

    updateUser(user: User): Observable<object> {
        return this.http.put(`${USER_URL}/${user.id}`, JSON.stringify(user), this.httpOptions);
    }

    getToken(): string {
        return this.storage.get(USER_TOKEN) ?? '';
    }

    setTokenInfo(res: TokenResponse): void {
        this.storage.set(USER_TOKEN, res.token);        
        this.storage.set(EXPIRES_AT, res.expires.toString());
    }

    setUserInfo(user: User) {
        this.storage.set(USER_ID, user.id.toString());
        this.storage.set(USER_EMAIL, user.email);        
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