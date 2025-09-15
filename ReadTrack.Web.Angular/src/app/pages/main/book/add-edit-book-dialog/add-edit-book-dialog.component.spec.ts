import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditBookDialogComponent } from './add-edit-book-dialog.component';

describe('AddEditBookDialogComponent', () => {
    let component: AddEditBookDialogComponent;
    let fixture: ComponentFixture<AddEditBookDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
    imports: [AddEditBookDialogComponent]
}).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(AddEditBookDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
