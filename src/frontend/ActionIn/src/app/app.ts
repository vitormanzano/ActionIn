import { Component, signal } from '@angular/core';
import { LoginPage } from './components/login-page/login-page';

@Component({
  selector: 'app-root',
  imports: [LoginPage],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('ActionIn');
}
