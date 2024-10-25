import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { Observable, Subject } from 'rxjs';
import UserAuthDTO from '../../shared/models/DTOs/UserAuthDTO';
import AvailableUserRole from '../../shared/models/enums/AvailableUserRole';

@Component({
  selector: 'app-header-menu',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    RouterModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './header-menu.component.html',
  styleUrl: './header-menu.component.scss',
})
export class HeaderMenuComponent implements OnInit {
  currentUser$!: Observable<UserAuthDTO>;
  constructor(private _authService: AuthService) {}

  ngOnInit(): void {
    this.fetchUser();
  }
  fetchUser() {
    this.currentUser$ = this._authService.user$;
  }
  isEmployee(user: UserAuthDTO) {
    return this._authService.isEmployee(user);
  }

  isEmployeeInGeneral(user: UserAuthDTO) {
    return this._authService.isEmployeeInGeneral(user);
  }

  isManagerInGeneral(user: UserAuthDTO) {
    return this._authService.isManagerInGeneral(user);
  }

  isAdmin(user: UserAuthDTO) {
    return this._authService.isAdmin(user);
  }

  logOut() {
    this._authService.logOut().subscribe(() => this.fetchUser());
  }
}
