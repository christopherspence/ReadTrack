import { Injectable } from '@angular/core';
import { BaseHttpService } from './base-http-service';
import { HttpClient } from '@angular/common/http';
import { Book, CreateBookRequest } from '../../shared';
import { Observable } from 'rxjs';

const BOOK_URL = 'book';

@Injectable({
    providedIn: 'root'
})
export class BookService extends BaseHttpService {
    constructor(http: HttpClient) {
        super(http);
    }

    getBookCount(searchText?: string): Observable<number> {
        searchText = searchText || '';

        return this.http.get<number>(`${BOOK_URL}/count/${searchText}`);
    }

    getBooks(offset: number, count: number, searchText?: string): Observable<Array<Book>> {
        searchText = searchText || '';
        return this.http.get<Array<Book>>(`${BOOK_URL}/${offset}/${count}/${searchText}`);
    }

    createBook(request: CreateBookRequest): Observable<Book> {
        return this.http.post<Book>(BOOK_URL, request);
    }

    updateBook(book: Book): Observable<object> {
        return this.http.put(`${BOOK_URL}/${book.id}`, `${JSON.stringify(book)}`, this.httpOptions);
    }

    deleteBook(id: number): Observable<object> {
        return this.http.delete(`${BOOK_URL}/${id}`);
    }
}