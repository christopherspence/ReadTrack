import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { SearchBoxComponent } from './search-box.component';

describe('SearchBoxComponent', () => {
    let component: SearchBoxComponent;
    let fixture: ComponentFixture<SearchBoxComponent>;

    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
    imports: [SearchBoxComponent]
}).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(SearchBoxComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
