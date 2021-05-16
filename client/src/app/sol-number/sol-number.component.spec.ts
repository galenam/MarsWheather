import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolNumberComponent } from './sol-number.component';

describe('SolNumberComponent', () => {
  let component: SolNumberComponent;
  let fixture: ComponentFixture<SolNumberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolNumberComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
