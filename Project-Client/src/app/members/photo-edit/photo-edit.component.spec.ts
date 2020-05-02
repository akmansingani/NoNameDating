/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PhotoEditComponent } from './photo-edit.component';

let component: PhotoEditComponent;
let fixture: ComponentFixture<PhotoEditComponent>;

describe('photo-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PhotoEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PhotoEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});