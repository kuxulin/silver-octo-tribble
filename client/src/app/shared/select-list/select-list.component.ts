import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import User from '../models/User';
import AvailableUserRole from '../models/enums/AvailableUserRole';
import { debounceTime, map, Observable, of, Subject } from 'rxjs';
import { UserService } from '../services/user.service';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-select-list',
  standalone: true,
  imports: [
    MatFormFieldModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    ReactiveFormsModule,
    FormsModule,
    MatListModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './select-list.component.html',
  styleUrl: './select-list.component.scss',
})
export class SelectListComponent implements OnInit {
  partialUserName = '';
  labelText = '';
  @Output()
  selectedUsersOutput = new EventEmitter<User[]>();
  selectedUsers: User[] = [];
  @Input()
  initialUsers: User[] = [];
  @Input()
  constantUsers: User[] = [];
  @Input({ required: true })
  userRole!: AvailableUserRole;
  @Input()
  single = false;
  @Input()
  enabledUsers!: User[];
  @Input()
  isManager!: boolean;
  users$!: Observable<User[]>;
  _destroy = new Subject<boolean>();
  constructor(private _userService: UserService) {}

  ngOnInit(): void {
    this.labelText =
      (this.userRole === AvailableUserRole.Manager
        ? AvailableUserRole[AvailableUserRole.Manager]
        : AvailableUserRole[AvailableUserRole.Employee]) + 's';

    this.selectedUsers.push(...this.initialUsers, ...this.constantUsers);
    this.selectedUsersOutput.emit(this.selectedUsers);
  }

  getUsers() {
    if (this.partialUserName === '') {
      this.users$ = of([]);
      return;
    }

    let collection$ = this._userService.getUsersInRoleWithName(
      this.userRole,
      this.partialUserName
    );

    if (this.enabledUsers?.length > 0)
      collection$ = of(
        this.enabledUsers.filter((u) =>
          u.userName?.includes(this.partialUserName)
        )
      );

    this.users$ = collection$.pipe(
      debounceTime(300),
      map((res) =>
        res.filter((user) => !this.selectedUsers.some((u) => u.id === user.id))
      )
    );
  }

  clearSelectionSearch() {
    this.partialUserName = '';
    this.users$ = of([]);
  }

  addUserToSelection(users: User[]) {
    if (users.length === 0) return;

    if (this.single) this.selectedUsers.pop();

    this.selectedUsers.push(users[0]);
    this.selectedUsersOutput.emit(this.selectedUsers);
    this.getUsers();
  }

  removeUserFromSelection(user: User) {
    if (this.constantUsers.some((u) => u.id === user.id) || !this.isManager)
      return;

    this.selectedUsers.splice(this.selectedUsers.indexOf(user), 1);
    this.selectedUsersOutput.emit(this.selectedUsers);
    this.getUsers();
  }

  ngOnDestroy() {
    this._destroy.next(true);
    this._destroy.complete();
  }
}
