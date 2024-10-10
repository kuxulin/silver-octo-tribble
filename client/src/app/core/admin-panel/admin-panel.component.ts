import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { UserService } from '../../shared/services/user.service';
import { CommonModule, DatePipe } from '@angular/common';
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
import { MatPaginatorModule } from '@angular/material/paginator';
import userQueryOptions from '../../shared/models/QueryOptions/UserQueryOptions';
import pagedResult from '../../shared/models/PagedResult';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import {
  MatDatepickerInputEvent,
  MatDatepickerModule,
} from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter, APP_DATE_FORMATS } from '../../shared/AppDateAdapter';
import {
  AppDateAdapter,
  APP_DATE_FORMATS,
} from '../../shared/adapters/AppDateAdapter';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import AvailableUserRole from '../../shared/models/enums/AvailableUserRole';

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

  displayedColumns = [
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
  private _optionsSubject = new BehaviorSubject<userQueryOptions>(this.options);
  result$!: Observable<pagedResult<User>>;

  constructor(private userService: UserService) {}
  filterRolesInput: string[] = [''];

  ngOnInit(): void {
    this._optionsSubject.pipe(takeUntil(this._destroy$)).subscribe((value) => {
      this.options = value;

      if (this.sort) {
        this.sort.active = this.options.sortField || '';
        this.sort.direction = this.options.sortByDescending ? 'desc' : 'asc';
      }

      this.result$ = this.userService.getAllUsers(value).pipe(shareReplay());
    });
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
  }

  deleteUser(id: string) {
    this.userService.deleteUser(id).subscribe(() => {
      console.log('deleted');
    }); //update users
  }
}
