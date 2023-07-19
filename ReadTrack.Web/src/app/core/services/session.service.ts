import { Injectable } from '@angular/core';
import { BaseHttpService } from './base-http-service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Session } from '../../shared';

const SESSION_URL = 'session';

@Injectable({
    providedIn: 'root'
})
export class SessionService extends BaseHttpService {
    constructor(http: HttpClient) {
        super(http);
    }

    getSessionCount(bookId: number, searchText?: string): Observable<number> {
        searchText = searchText || '';

        return this.http.get<number>(`${SESSION_URL}/count/${bookId}/${searchText}`);
    }

    getSessions(bookId: number, offset: number, count: number, searchText?: string): Observable<Array<Session>> {
        searchText = searchText || '';
        return this.http.get<Array<Session>>(`${SESSION_URL}/${bookId}/${offset}/${count}/${searchText}`);
    }

    createSession(session: Session): Observable<Session> {
        return this.http.post<Session>(SESSION_URL, session);
    }

    updateSession(session: Session): Observable<object> {
        return this.http.put(`${SESSION_URL}/${session.id}`, `${JSON.stringify(session)}`, this.httpOptions);
    }

    deleteSession(id: number): Observable<object> {
        return this.http.delete(`${SESSION_URL}/${id}`);
    }
}