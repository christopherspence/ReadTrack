import { HttpClient, HttpHeaders } from '@angular/common/http';

export abstract class BaseHttpService {
    httpOptions = {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
    };

    constructor(protected http: HttpClient) {}
}
