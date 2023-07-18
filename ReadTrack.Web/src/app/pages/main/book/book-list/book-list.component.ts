import { Component, OnInit, ViewChild } from '@angular/core';
import { Book } from '../../../../shared';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { BookService } from '../../../../core/services';

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
    
    constructor(private service: BookService) {
        
    }

    async ngOnInit(): Promise<void> {
        this.getBooks();
    }

    async getBooks(): Promise<void> {
        this.count = await this.service.getBookCount(this.searchValue).toPromise() ?? 0;
        this.books = await this.service.getBooks(this.offset * this.limit,
            this.limit,
            this.searchValue).toPromise() ?? [];
    }

    addBook(): void {

    }

    deleteBook(event: any): void {

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

    async toggleFinished(event: any): Promise<void> {

    }
}