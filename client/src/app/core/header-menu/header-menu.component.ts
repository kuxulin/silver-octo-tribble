import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { Observable, Subscription, tap } from 'rxjs';
import UserAuthDTO from '../../shared/models/DTOs/UserAuthDTO';
import { MatMenuModule } from '@angular/material/menu';
import Change from '../../shared/models/Change';
import { ChangeService } from '../../shared/services/change.service';
import { NotificationsService } from '../../shared/services/notifications.service';

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
  currentUserValue!: UserAuthDTO;
  managerChanges$!: Observable<Change[]>;
  employeeChanges$!: Observable<Change[]>;
  onCreated!: Subscription;
  constructor(
    private _authService: AuthService,
    private _changeService: ChangeService,
    private _router: Router,
    private _notificationsService: NotificationsService
  ) {}

  ngOnInit(): void {
    this.fetchUser();
    this.onCreated = this._notificationsService.onChangeCreatedEvent.subscribe(
      () => this.fetchChanges()
    );
  }

  fetchUser() {
    this.currentUser$ = this._authService.user$.pipe(
      tap((res) => {
        this.currentUserValue = res;
        this.fetchChanges();
      })
    );
  }

  fetchChanges() {
    if (!!this.currentUserValue.managerId)
      this.managerChanges$ = this._changeService.getChangesFromManager(
        this.currentUserValue.managerId
      );

    if (!!this.currentUserValue.employeeId)
      this.employeeChanges$ = this._changeService.getChangesFromEmployee(
        this.currentUserValue.employeeId
      );
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

  navigateToProject(change: Change) {
    if (!change.isRead)
      this._changeService
        .makeChangeRead(change.id, this.currentUserValue.id)
        .subscribe(() => {
          this.fetchChanges();
        });

    this._router.navigate(['dashboard/'], {
      queryParams: { selectedProjectId: change.projectId },
    });
  }

  logOut() {
    this._authService.logOut().subscribe(() => this.fetchUser());
  }

  ngOnDestroy() {
    this.onCreated.unsubscribe();
  }
}
