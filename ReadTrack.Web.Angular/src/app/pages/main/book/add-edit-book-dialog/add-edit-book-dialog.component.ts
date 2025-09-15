import { Component, Inject, OnInit } from '@angular/core';
import { Book, CreateBookRequest, DialogMode } from '../../../../shared';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookService } from 'src/app/core/services';
import { BookCategory } from 'src/app/shared/models/book-category.model';

@Component({
    selector: 'app-add-edit-book-dialog',
    templateUrl: './add-edit-book-dialog.component.html',
    styleUrls: ['./add-edit-book-dialog.component.css']
})
export class AddEditBookDialogComponent implements OnInit {
    title = '';
    mode: DialogMode;
    form!: FormGroup;
    categories: Array<string> = Object.values(BookCategory);
    
    get book(): Book {
        const form = this.form?.value;

        const book = new Book(
            form.id,
            form.name,
            form.author,
            form.category,
            form.numberOfPages,
            form.finished,
            [],
            form.published);
        
        return book;
    }

    constructor(
        public dialogRef: MatDialogRef<AddEditBookDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private service: BookService,
        private fb: FormBuilder,
        private snackBar: MatSnackBar) {
        this.mode = data.mode;

        this.setupForm();

        if (this.mode == DialogMode.edit) {
            this.fillData(data.book);
        }
    }

    ngOnInit(): void {
        this.setTitle();
    }

    private setTitle(): void {
        const titleArr = [];

        if (this.mode === DialogMode.add) {
            titleArr.push('Add');
        } else if (this.mode === DialogMode.edit) {
            titleArr.push('Edit');
        }

        titleArr.push('Book');

        this.title = titleArr.join(' ');
    }

    private setupForm(): void {
        this.form = this.fb.group({
            id: [0],
            name: ['', Validators.required],
            author: ['', Validators.required],
            published: [''],
            category: ['', Validators.required],
            numberOfPages: [0, Validators.required],
            finished: [false, Validators.required]            
        });
    }
    
    private fillData(book: Book) {
        this.form?.patchValue({
            id: book.id,
            name: book.name,
            author: book.author,
            published: book.published,
            category: book.category,
            numberOfPages: book.numberOfPages,
            finished: book.finished
        });
    }

    private showToast(action: string, id: number): void {
        this.snackBar.open(`Book ${action}`, `${id}`, {
            duration: 2000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
        });
    }

    async addBook(): Promise<void> {
        const form = this.form?.value;

        const request = new CreateBookRequest(form.name, form.author, form.category, form.numberOfPages, form.published);
        
        try {
            const id = (await this.service.createBook(request).toPromise())?.id;

            if (id) {
                this.showToast('Added', id);
                this.dialogRef.close(true);
            }
        } catch {
            this.dialogRef.close(false);                
        }            
             
    }

    async updateBook(): Promise<void> {
        const book = this.book;

        if (book) {
            try {
                await this.service.updateBook(book).toPromise()

                this.showToast('Updated', book.id);
                this.dialogRef.close(true);
            } catch {
                this.dialogRef.close(false);                
            }            
        }        
    }

    async submit(): Promise<void> {
        if (this.form.valid) {
            if (this.mode === DialogMode.add) {
                await this.addBook();
            } else if (this.mode === DialogMode.edit) {
                await this.updateBook();
            }
        }
    }
}