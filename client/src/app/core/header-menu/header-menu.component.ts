import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { Observable, tap } from 'rxjs';
import UserAuthDTO from '../../shared/models/DTOs/UserAuthDTO';
import { MatMenuModule } from '@angular/material/menu';
import Change from '../../shared/models/Change';
import { ChangeService } from '../../shared/services/change.service';

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
    MatMenuModule,
  ],
  templateUrl: './header-menu.component.html',
  styleUrl: './header-menu.component.scss',
})
export class HeaderMenuComponent implements OnInit {
  currentUser$!: Observable<UserAuthDTO>;
  managerChanges$!: Observable<Change[]>;
  employeeChanges$!: Observable<Change[]>;
  constructor(
    private _authService: AuthService,
    private _changeService: ChangeService,
    private _router: Router
  ) {}

  ngOnInit(): void {
    this.fetchUser();
  }

  fetchUser() {
    this.currentUser$ = this._authService.user$.pipe(
      tap((res) => this.fetchChanges(res.employeeId, res.managerId))
    );
  }

  fetchChanges(employeeId: string | undefined, managerId: string | undefined) {
    if (!!managerId)
      this.managerChanges$ =
        this._changeService.getChangesFromManager(managerId);

    if (!!employeeId)
      this.employeeChanges$ =
        this._changeService.getChangesFromEmployee(employeeId);
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

  convertChange(change: Change) {
    return this._changeService.convertChange(change);
  }

  navigateToProject(projectId: string) {
    this._router.navigate(['dashboard/'], {
      queryParams: { selectedProjectId: projectId },
    });
  }

  logOut() {
    this._authService.logOut().subscribe(() => this.fetchUser());
  }
}
