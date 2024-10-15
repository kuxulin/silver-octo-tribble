import { Routes } from '@angular/router';
import { UserProfileComponent } from './core/user-profile/user-profile.component';
import { LoginComponent } from './core/login/login.component';
import { RegisterComponent } from './core/register/register.component';
import { AdminPanelComponent } from './core/admin-panel/admin-panel.component';
import { DashboardComponent } from './core/dashboard/dashboard.component';

export const routes: Routes = [
  { path: 'user/:id', component: UserProfileComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'panel', component: AdminPanelComponent },
  { path: 'dashboard', component: DashboardComponent },
];
