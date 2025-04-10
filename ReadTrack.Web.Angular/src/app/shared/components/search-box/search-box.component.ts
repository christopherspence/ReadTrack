import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Subject, debounceTime } from 'rxjs';

@Component({
    selector: 'app-search-box',
    standalone: false,
    templateUrl: './search-box.component.html',
    styleUrls: ['./search-box.component.css']
})
export class SearchBoxComponent implements OnInit {
    private subject = new Subject<string>();
    
    @Input() value: string = '';
    @Output() filter = new EventEmitter<string>();

    async ngOnInit(): Promise<void> {
        this.value = this.value || '';
        this.subject.pipe(debounceTime(300)).subscribe(value => {
            this.filter.emit(value);
        });
    }

    onKeyup(event: Event): void {
        const value = (event.target as HTMLInputElement).value.toLowerCase();

        this.subject.next(value);
    }
}