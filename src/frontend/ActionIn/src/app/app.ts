import { Component, signal } from '@angular/core';
import { LoginPage } from './auth/components/login-page/login-page';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [LoginPage, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('ActionIn');
}
