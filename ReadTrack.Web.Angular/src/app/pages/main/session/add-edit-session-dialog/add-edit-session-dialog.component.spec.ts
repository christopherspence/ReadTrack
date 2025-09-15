import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditSessionDialogComponent } from './add-edit-session-dialog.component';

describe('AddEditSessionDialogComponent', () => {
    let component: AddEditSessionDialogComponent;
    let fixture: ComponentFixture<AddEditSessionDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
    imports: [AddEditSessionDialogComponent]
}).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(AddEditSessionDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
