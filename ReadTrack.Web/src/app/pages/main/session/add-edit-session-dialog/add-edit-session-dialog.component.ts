import { Component, Inject, OnInit } from '@angular/core';
import { Session, DialogMode } from '../../../../shared';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SessionService } from '../../../../core/services';
import { CreateSessionRequest } from '../../../../shared';

@Component({
    selector: 'app-add-edit-session-dialog',
    templateUrl: './add-edit-session-dialog.component.html',
    styleUrls: ['./add-edit-session-dialog.component.css']
})
export class AddEditSessionDialogComponent implements OnInit {
    bookId = 0;
    title = '';
    mode: DialogMode;
    form!: FormGroup;
    
    padNumber(number: number): string {   
        return `${(number < 10 ? '0' : '')}${number}`;      
   }

   get duration(): string {
       const form = this.form?.value;
       return `${this.padNumber(Number(form.hours))}:${this.padNumber(Number(form.minutes))}:${this.padNumber(Number(form.seconds))}`;
   }

    get session(): Session {
        const form = this.form?.value;

        const session = new Session(
            form.id,
            form.date,
            this.duration,
            form.numberOfPages,
            form.startPage,
            form.endPage);
        
        return session;
    }

    constructor(
        public dialogRef: MatDialogRef<AddEditSessionDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private service: SessionService,
        private fb: FormBuilder,
        private snackBar: MatSnackBar) {
        this.mode = data.mode;

        if (data.bookId) {
            this.bookId = data.bookId;
        }

        this.setupForm();

        if (this.mode == DialogMode.edit) {
            this.fillData(data.session);
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

        titleArr.push('Session');

        this.title = titleArr.join(' ');
    }

    private setupForm(): void {
        this.form = this.fb.group({
            id: [0],
            date: ['', Validators.required],
            hours: [0, Validators.required],
            minutes: [0, Validators.required],
            seconds: [0, Validators.required],
            numberOfPages: [0, Validators.required],
            startPage: [0, Validators.required],
            endPage: [0, Validators.required]
        });
    }
    
    private fillData(session: Session) {
        const durationArr = session.duration.split(':');

        this.form?.patchValue({
            id: session.id,
            date: session.date,
            hours: Number(durationArr[0]),
            minutes: Number(durationArr[1]),
            seconds: Number(durationArr[2]),
            numberOfPages: session.numberOfPages,
            startPage: session.startPage,
            endPage: session.endPage
        });
    }
    
    calculateNumberOfPages(): void {
        const form = this.form.value
        const numberOfPages = Number(form.endPage) - Number(form.startPage);

        if (numberOfPages > 0) {
            this.form.patchValue({ numberOfPages });        
        }
    }

    private showToast(action: string, id: number): void {
        this.snackBar.open(`Session ${action}`, `${id}`, {
            duration: 2000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
        });
    }

    async addSession(): Promise<void> {
        const form = this.form.value;

        const request = new CreateSessionRequest(
            this.bookId, 
            form.date, 
            form.duration, 
            form.numberOfPages, 
            form.startPage,
            form.endPage);

        if (Session) {
            try {
                const id = (await this.service.createSession(this.bookId, request).toPromise())?.id;

                if (id) {
                    this.showToast('Added', id);
                    this.dialogRef.close(true);
                }
            } catch {
                this.dialogRef.close(false);                
            }            
        }        
    }

    async updateSession(): Promise<void> {
        const session = this.session;

        if (session) {
            try {
                await this.service.updateSession(session).toPromise()

                this.showToast('Updated', session.id);
                this.dialogRef.close(true);
            } catch {
                this.dialogRef.close(false);                
            }            
        }        
    }

    async submit(): Promise<void> {
        if (this.form.valid) {
            if (this.mode === DialogMode.add) {
                await this.addSession();
            } else if (this.mode === DialogMode.edit) {
                await this.updateSession();
            }
        }
    }
}