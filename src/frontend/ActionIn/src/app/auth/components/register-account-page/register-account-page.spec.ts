import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterAccountPage } from './register-account-page';

describe('RegisterAccountPage', () => {
  let component: RegisterAccountPage;
  let fixture: ComponentFixture<RegisterAccountPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterAccountPage],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterAccountPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
