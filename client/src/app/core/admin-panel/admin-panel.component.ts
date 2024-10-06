import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
import { MatPaginatorModule } from '@angular/material/paginator';
import userQueryOptions from '../../shared/models/QueryOptions/UserQueryOptions';
import pagedResult from '../../shared/models/PagedResult';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    MatTableModule,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss',
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  @ViewChild(MatSort) sort!: MatSort;
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
  result$!: Observable<pagedResult<User>>;
  private _destroy$ = new Subject<boolean>();

  options: userQueryOptions = {
    partialUsername: undefined,
    filterRoles: undefined,
    isBlocked: undefined,
    startDate: undefined,
    endDate: undefined,
    pageSize: 5,
    pageIndex: 0,
    sortField: undefined,
    sortByDescending: undefined,
  };

  private _optionsSubject = new BehaviorSubject<userQueryOptions>(this.options);

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this._optionsSubject.pipe(takeUntil(this._destroy$)).subscribe((value) => {
      this.options = value;

      if (this.sort) {
        this.sort.active = this.options.sortField || '';
        this.sort.direction = this.options.sortByDescending ? 'desc' : 'asc';
      }

      this.result$ = this.userService.getAllUsers(value).pipe(shareReplay());
    });
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

  updateQuery(newOptions: Partial<userQueryOptions>) {
    this._optionsSubject.next({ ...this._optionsSubject.value, ...newOptions });
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
    this._destroy$.complete();
  }
}
