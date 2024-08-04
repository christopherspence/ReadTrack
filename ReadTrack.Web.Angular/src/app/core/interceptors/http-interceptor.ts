import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, of, throwError } from 'rxjs';
import { UserService } from '../services';
import { environment } from 'src/environments/environment';

enum HTTP_STATUS {
    SUCCESS = 200,
    UNAUTHORIZED = 401,
    FORBIDDEN = 403
}

@Injectable()
export class ReadTrackHttpInterceptor implements HttpInterceptor {
    constructor(
        private userService: UserService,
        private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        req = req.clone({
            url: `${environment.baseUrl}/${req.url}`
        });

        const token = this.userService.getToken();
        if (token) {
            req = req.clone({
                headers: req.headers.set('authorization', `Bearer ${token}`)
            });
        }

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === HTTP_STATUS.SUCCESS) {
                    const body = error.error.text || '';
                    const resp = new HttpResponse({
                        body
                    });
                    return of(resp);
                } else {
                    if (error.status === HTTP_STATUS.UNAUTHORIZED || error.status === HTTP_STATUS.FORBIDDEN) {
                        this.userService.logout();
                        this.router.navigate(['login']);
                    }

                    let message = '';

                    if (typeof error.error?.error === 'string') {
                        message = error.error.error;
                    } else if (error.error && error.error.error && error.error.error.message && error.error.error.message.length) {
                        message = error.error.error.message;
                    } else {
                        message = error.statusText;
                    }

                    return throwError(error);
                }
            })
        );
    }    
}
