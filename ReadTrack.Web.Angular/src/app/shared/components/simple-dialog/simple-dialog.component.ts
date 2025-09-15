import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-validation-dialog',
    templateUrl: './simple-dialog.component.html',
    styleUrls: ['./simple-dialog.component.scss'],
    standalone: true,
    imports: [MatButtonModule, MatDialogModule]
})
export class SimpleDialogComponent {

    constructor(
        public dialogRef: MatDialogRef<SimpleDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) {
        data.okText = data.okText || 'OK';
    }
}
