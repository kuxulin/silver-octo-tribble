import { Routes } from '@angular/router';
import { InitialComponent } from './core/initial/initial.component';
import { LoginComponent } from './core/login/login.component';
import { RegisterComponent } from './core/register/register.component';
import { AdminPanelComponent } from './core/admin-panel/admin-panel.component';

export const routes: Routes = [
  { path: 'initial', component: InitialComponent },
  { path: '', redirectTo: '/initial', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'panel', component: AdminPanelComponent },
];
