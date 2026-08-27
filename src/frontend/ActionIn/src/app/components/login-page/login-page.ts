import { Component } from '@angular/core';
import { Header } from '../../shared/header/header';
import { LoginForm } from './login-form/login-form';

@Component({
  imports: [Header, LoginForm],
  selector: 'app-login-page',
  styleUrl: './login-page.css',
  templateUrl: './login-page.html',
})
export class LoginPage {}
