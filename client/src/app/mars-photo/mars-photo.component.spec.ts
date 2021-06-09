import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarsPhotoComponent } from './mars-photo.component';

describe('MarsPhotoComponent', () => {
  let component: MarsPhotoComponent;
  let fixture: ComponentFixture<MarsPhotoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MarsPhotoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MarsPhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
