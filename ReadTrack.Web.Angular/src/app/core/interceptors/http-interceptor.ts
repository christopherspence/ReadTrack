import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, of, throwError } from 'rxjs';
import { UserService } from '../services';
import { environment } from 'src/environments/environment';

enum HTTP_STATUS {
    SUCCESS = 200,
    UNAUTHORIZED = 401,
    FORBIDDEN = 403
}


export const readTrackHttpInterceptor: HttpInterceptorFn = (req, next) => {
    const userService = inject(UserService);
    req = req.clone({
        url: `${environment.baseUrl}/${req.url}`
    });

    const token = userService.getToken();
    if (token) {
        req = req.clone({
            headers: req.headers.set('authorization', `Bearer ${token}`)
        });
    }

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === HTTP_STATUS.SUCCESS) {
                const body = error.error.text || '';
                const resp = new HttpResponse({
                    body
                });
                return of(resp);
            } else {
                if (error.status === HTTP_STATUS.UNAUTHORIZED || error.status === HTTP_STATUS.FORBIDDEN) {
                    userService.logout();
                    const router = inject(Router);
                    router.navigate(['login']);
                }

                let message = '';

                if (typeof error.error?.error === 'string') {
                    message = error.error.error;
                } else if (error.error && error.error.error && error.error.error.message && error.error.error.message.length) {
                    message = error.error.error.message;
                } else {
                    message = error.statusText;
                }

                return throwError(() => error);
            }
        })
    );
};
