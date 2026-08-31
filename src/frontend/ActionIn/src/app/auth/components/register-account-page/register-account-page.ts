import { Component } from '@angular/core';
import { Header } from '../../../shared/header/header';
import { RegisterAccountForm } from './register-account-form/register-account-form';

@Component({
  imports: [Header, RegisterAccountForm],
  selector: 'app-register-account-page',
  styleUrl: './register-account-page.css',
  templateUrl: './register-account-page.html',
})
export class RegisterAccountPage {}
