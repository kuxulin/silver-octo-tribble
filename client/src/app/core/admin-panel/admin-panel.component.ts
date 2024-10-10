import { Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { UserService } from '../../shared/services/user.service';
import { CommonModule } from '@angular/common';
import User from '../../shared/models/User';
import {
  BehaviorSubject,
  Observable,
  shareReplay,
  Subject,
  takeUntil,
} from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import UserQueryOptions from '../../shared/models/QueryOptions/UserQueryOptions';
import PagedResult from '../../shared/models/PagedResult';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import {
  AppDateAdapter,
  APP_DATE_FORMATS,
} from '../../shared/adapters/AppDateAdapter';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import AvailableUserRole from '../../shared/models/enums/AvailableUserRole';
import { SelectionModel } from '@angular/cdk/collections';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    MatTableModule,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatSlideToggleModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    FormsModule,
    MatCheckboxModule,
  ],
  providers: [
    { provide: DateAdapter, useClass: AppDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS },
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss',
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject<boolean>();
  @ViewChild(MatSort) sort!: MatSort;
  showButton = false;
  readonly range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });
  readonly dialog = inject(MatDialog);
  selection = new SelectionModel<User>(true, []);

  displayedColumns = [
    'select',
    'userName',
    'firstName',
    'lastName',
    'phoneNumber',
    'creationDate',
    'roles',
    'blocked',
    'actions',
  ];

  options: userQueryOptions = {
  options: UserQueryOptions = {
    partialUserName: undefined,
    filterRoles: undefined,
    isBlocked: undefined,
    startDate: undefined,
    endDate: undefined,
    pageSize: 5,
    pageIndex: 0,
    sortField: undefined,
    sortByDescending: undefined,
  };

  partialUserNameInput: string = '';
  filterRolesInput: string[] = [''];
  private _optionsSubject = new BehaviorSubject<UserQueryOptions>(this.options);
  result$!: Observable<PagedResult<User>>;
  constructor(private _userService: UserService) {}

  ngOnInit(): void {
    this._optionsSubject.pipe(takeUntil(this._destroy$)).subscribe((value) => {
      this.options = value;

      if (this.sort) {
        this.sort.active = this.options.sortField || '';
        this.sort.direction = this.options.sortByDescending ? 'desc' : 'asc';
      }

      this.result$ = this._userService.getAllUsers(value).pipe(shareReplay());
    });
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.paginator.pageSize;
    return numSelected === numRows;
  }

  toggleAllRows(data: User[]) {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...data);
  }

  displayButton(marker: boolean) {
    this.showButton = marker;
  }
  }

  clearPartialUsername() {
    this.updateQuery({ partialUserName: undefined });
    this.partialUserNameInput = '';
  }

  onSortChange(sort: Sort) {
    if (sort.active && !sort.direction) {
      this.updateQuery({ sortField: undefined, sortByDescending: undefined });
      return;
    }

    this.updateQuery({
      sortField: sort.active,
      sortByDescending: sort.direction === 'desc',
    });
  }

  clearDate() {
    this.updateQuery({ startDate: undefined, endDate: undefined });
    this.range.reset();
  }

  updateQuery(newOptions: Partial<userQueryOptions>) {
  searchRoles() {
    if (!this.filterRolesInput.some((r) => r.length > 0)) {
      this.updateQuery({ filterRoles: undefined });
      return;
    }

    let enumRoles = this.filterRolesInput.map(
      (role) =>
        AvailableUserRole[
          (role.charAt(0).toUpperCase() +
            role.slice(1).toLowerCase()) as keyof typeof AvailableUserRole
        ]
    );

    enumRoles = enumRoles.filter((en) => !!en);

    this.updateQuery({ filterRoles: enumRoles });
  }
    this._optionsSubject.next({ ...this._optionsSubject.value, ...newOptions });
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
    this._destroy$.complete();

  triggerDeleteDialog(id: string) {
    let dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        action: 'delete',
        entity: 'user(s)',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.deleteUser(id);
      }
    });
  }

  deleteUser(id: string) {
    let ids = this.selection.selected.map((u) => u.id!);

    if (!ids.includes(id)) ids.push(id);

    this._userService.deleteUsers(ids).subscribe(() => {
      this.updateQuery(this.options);
      this._metricsChangedSubject.next(true);
    });
  }
  }
}
