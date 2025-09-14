import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DutyDialog } from './duty-dialog';

describe('DutyDialog', () => {
  let component: DutyDialog;
  let fixture: ComponentFixture<DutyDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DutyDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DutyDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
