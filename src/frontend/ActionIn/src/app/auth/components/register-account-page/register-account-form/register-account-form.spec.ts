import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterAccountForm } from './register-account-form';

describe('RegisterAccountForm', () => {
  let component: RegisterAccountForm;
  let fixture: ComponentFixture<RegisterAccountForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterAccountForm],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterAccountForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
