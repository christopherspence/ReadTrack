import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TimeSegment } from '../../shared';

const ANALYTICS_URL = 'analytics';

@Injectable({
    providedIn: 'root'    
})
export class AnalyticsService {
    constructor(
        private datePipe: DatePipe,
        private http: HttpClient) { }

    booksRead(start: Date, end: Date): Observable<Array<TimeSegment<number>>> {
        const startStr = this.datePipe.transform(start, 'MM-dd-yyyy');
        const endStr = this.datePipe.transform(end, 'MM-dd-yyyy');

        return this.http.get<Array<TimeSegment<number>>>(`${ANALYTICS_URL}/books/${startStr}/${endStr}`);
    }

    readingTime(start: Date, end: Date): Observable<Array<TimeSegment<number>>> {
        const startStr = this.datePipe.transform(start, 'MM-dd-yyyy');
        const endStr = this.datePipe.transform(end, 'MM-dd-yyyy');

        return this.http.get<Array<TimeSegment<number>>>(`${ANALYTICS_URL}/time/${startStr}/${endStr}`);
    }
}