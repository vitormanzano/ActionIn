import { Routes } from '@angular/router';
import { LoginPage } from './components/login-page/login-page';
import { RegisterAccountPage } from './components/register-account-page/register-account-page';

export const authRoutes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginPage,
  },
  {
    path: 'register',
    component: RegisterAccountPage,
  },
];
