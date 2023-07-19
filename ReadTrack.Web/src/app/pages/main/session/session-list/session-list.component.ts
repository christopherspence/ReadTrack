import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Session } from '../../../../shared';
import { ActivatedRoute } from '@angular/router';
import { SessionService } from 'src/app/core/services/session.service';

@Component({
    selector: 'app-session-list',
    templateUrl: './session-list.component.html',
    styleUrls: ['./session-list.component.css']
})
export class SessionListComponent implements OnInit {
    @ViewChild(DatatableComponent) myTable?: DatatableComponent;

    id: number;

    limit = 10;
    offset = 0;
    count = 0;
    searchValue = '';

    sessions: Array<Session> = [];

    constructor(
        private route: ActivatedRoute,
        private service: SessionService) {
        this.id = Number(this.route.snapshot.params['id']);
    }

    async ngOnInit(): Promise<void> {
        await this.getSessions();
    }

    async getSessions(): Promise<void> {
        this.count = await this.service.getSessionCount(this.id, this.searchValue).toPromise() ?? 0;
        this.sessions = await this.service.getSessions(
            this.id,
            this.offset * this.limit,
            this.limit,
            this.searchValue).toPromise() ?? [];
    }

    async createBook(): Promise<void> {
        throw new Error('Method not implemented.');
    }

    async editBook(row: any): Promise<void> {
        throw new Error('Method not implemented.');
    }

    async deleteBook(row: any): Promise<void> {
        throw new Error('Method not implemented.');
    }

    async updateFilter(searchText: string): Promise<void> {
        this.searchValue = searchText;

        await this.getSessions();
    }

    async onPage(event: any): Promise<void> {
        this.limit = event.limit;
        this.offset = event.offset;
        await this.getSessions();
    }

}