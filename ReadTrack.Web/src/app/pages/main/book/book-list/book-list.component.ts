import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Book, ConfirmDialogComponent, DialogMode, SimpleDialogComponent } from '../../../../shared';
import { BookService } from '../../../../core/services';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AddEditBookDialogComponent } from '../add-edit-book-dialog-component';

@Component({
    selector: 'app-book-list',
    templateUrl: './book-list.component.html',
    styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
    @ViewChild(DatatableComponent) myTable?: DatatableComponent;

    limit = 10;
    offset = 0;
    count = 0;
    searchValue = '';
    
    books: Array<Book> = [];
    
    constructor(
        private service: BookService,
        private snackBar: MatSnackBar,
        public dialog: MatDialog) { }

    async ngOnInit(): Promise<void> {
        this.getBooks();
    }

    async getBooks(): Promise<void> {
        this.count = await this.service.getBookCount(this.searchValue).toPromise() ?? 0;
        this.books = await this.service.getBooks(this.offset * this.limit,
            this.limit,
            this.searchValue).toPromise() ?? [];
    }

    async createBook(): Promise<void> {
        const dialogRef = this.dialog.open(AddEditBookDialogComponent, {
            width: '50%',
            data: {
                mode: DialogMode.add
            }
        });
        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            await this.getBooks();
        } 
    }

    async editBook(row: any): Promise<void> {
        const book = this.books.find(b => b.id === row);
        const dialogRef = this.dialog.open(AddEditBookDialogComponent, {
            width: '50%',
            data: {                
                mode: DialogMode.edit,
                book: book
            }
        });
        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            await this.getBooks();
        }
    }

    async deleteBook(row: any): Promise<void> {
        const book = this.books.find(b => b.id === row.id);
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            width: '300px',
            data: {
                title: 'Confirmation',
                message: `Are you sure you wish to delete ${book?.name}?`,
                destructive: true,
                confirmText: 'Delete'
            }
        });

        const result = await dialogRef.afterClosed().toPromise();

        if (result) {
            const response = await this.service.deleteBook(row.id).toPromise();
            if (response === null) {
                this.snackBar.open('Book Deleted', row.id, {
                    duration: 2000,
                    verticalPosition: 'top',
                    horizontalPosition: 'right',
                });

                await this.getBooks();
            } else {
                this.dialog.open(SimpleDialogComponent, {
                    width: '250px',
                    data: {
                        title: 'Error',
                        message: 'An error occurred deleting this book'
                    }
                });                
            }
        }
    }

    async updateFilter(searchText: string): Promise<void> {
        this.searchValue = searchText;

        await this.getBooks();
    }

    async onPage(event: any): Promise<void> {
        this.limit = event.limit;
        this.offset = event.offset;
        await this.getBooks();
    }

    async toggleFinished(event: MatCheckboxChange): Promise<void> {
        const book = this.books.find(b => b.id === Number(event.source.id));
        
        if (book) {
            book.finished = event.checked;

            await this.service.updateBook(book).toPromise();

            this.snackBar.open('Book updated', event.source.id, {
                duration: 2000,
                verticalPosition: 'top',
                horizontalPosition: 'right'
            });
        }
    }
}