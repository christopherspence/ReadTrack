import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent, NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ConfirmDialogComponent, DialogMode, Session, SimpleDialogComponent } from '../../../../shared';
import { ActivatedRoute } from '@angular/router';
import { SessionService } from 'src/app/core/services/session.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AddEditSessionDialogComponent } from '../add-edit-session-dialog';
import { DatePipe } from '@angular/common';
import { SearchBoxComponent } from '../../../../shared/components/search-box/search-box.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-session-list',
    templateUrl: './session-list.component.html',
    styleUrls: ['./session-list.component.css'],
    standalone: true,
    imports: [
        DatePipe, 
        MatButtonModule, 
        MatDialogModule, 
        MatSnackBarModule, 
        NgxDatatableModule, 
        SearchBoxComponent
    ]
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
        private service: SessionService,
        public dialog: MatDialog,
        private snackBar: MatSnackBar) {
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

    async createSession(): Promise<void> {
        const dialogRef = this.dialog.open(AddEditSessionDialogComponent, {
            width: '50%',
            data: {
                bookId: this.id,
                mode: DialogMode.add
            }
        });
        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            await this.getSessions();
        } 
    }

    async editSession(row: any): Promise<void> {
        const session = this.sessions.find(s => s.id === row);
        const dialogRef = this.dialog.open(AddEditSessionDialogComponent, {
            width: '50%',
            data: {                
                mode: DialogMode.edit,
                session: session
            }
        });
        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            await this.getSessions();
        }
    }

    async deleteSession(row: any): Promise<void> {
        const session = this.sessions.find(s => s.id === row.id);
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            width: '300px',
            data: {
                title: 'Confirmation',
                message: `Are you sure you wish to delete session ${session?.id}?`,
                destructive: true,
                confirmText: 'Delete'
            }
        });

        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            const response = await this.service.deleteSession(row.id).toPromise();
            if (response === null) {
                this.snackBar.open('Session Deleted', row.id, {
                    duration: 2000,
                    verticalPosition: 'top',
                    horizontalPosition: 'right',
                });

                await this.getSessions();
            } else {
                this.dialog.open(SimpleDialogComponent, {
                    width: '250px',
                    data: {
                        title: 'Error',
                        message: 'An error occurred deleting this session'
                    }
                });                
            }
        }
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