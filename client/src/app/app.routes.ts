import { Routes } from '@angular/router';
import { InitialComponent } from './core/initial/initial.component';
import { LoginComponent } from './core/login/login.component';
import { RegisterComponent } from './core/register/register.component';

export const routes: Routes = [
  { path: 'initial', component: InitialComponent },
  { path: '', component: InitialComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];
