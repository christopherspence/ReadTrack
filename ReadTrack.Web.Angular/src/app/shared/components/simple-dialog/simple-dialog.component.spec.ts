import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { SimpleDialogComponent } from './simple-dialog.component';

describe('ValidationDialogComponent', () => {
    let component: SimpleDialogComponent;
    let fixture: ComponentFixture<SimpleDialogComponent>;

    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [SimpleDialogComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(SimpleDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
