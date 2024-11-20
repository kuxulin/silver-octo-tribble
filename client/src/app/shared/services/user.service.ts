import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, shareReplay, take, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import UserQueryOptions from '../models/queryOptions/UserQueryOptions';
import PagedResult from '../models/PagedResult';
import User from '../models/User';
import AvailableUserRole from '../models/enums/AvailableUserRole';
import UsersMetrics from '../models/UserMetrics';
import UserUpdateDTO from '../models/DTOs/UserUpdateDTO';
import { ImageService } from './image.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _apiUrl = environment.server + 'api/User';

  constructor(
    private _httpClient: HttpClient,
    private _imageService: ImageService
  ) {}

  getUserById(id: number): Observable<User> {
    return this._httpClient
      .get<User>(this._apiUrl + '/' + id)
      .pipe(
        tap((user) =>
          this._imageService
            .getOriginalImage(user.imageId)
            .subscribe((image) => (user.image = image))
        )
      );
  }

  getAllUsers(options: UserQueryOptions): Observable<PagedResult<User>> {
    let params = this.createHttpParams(options);
    return this._httpClient
      .get<PagedResult<User>>(this._apiUrl, {
        params: params,
      })
      .pipe(
        tap((result) => {
          result.items.forEach((user) => {
            this._imageService
              .getOriginalImage(user.imageId)
              .subscribe((image) => (user.image = image));
          });
        }),
        map((result) => {
          result.items.forEach((user: any) => {
            user.creationDate = new Date(user.creationDate.toString() + 'Z');
            user.roles = user.roleIds;
          });
          return result;
        })
      );
  }

  private createHttpParams(options: Partial<UserQueryOptions>): HttpParams {
    let params = new HttpParams();

    if (options.pageIndex !== undefined)
      params = params.set('pageIndex', options.pageIndex.toString());

    if (options.pageSize !== undefined)
      params = params.set('pageSize', options.pageSize.toString());

    if (options.sortField !== undefined)
      params = params.set('sortField', options.sortField);

    if (options.sortByDescending !== undefined)
      params = params.set(
        'sortByDescending',
        options.sortByDescending.toString()
      );

    if (options.partialUserName !== undefined)
      params = params.set('partialUserName', options.partialUserName);

    if (options.filterRoles !== undefined) {
      options.filterRoles.forEach((role) => {
        params = params.append('filterRoleIds', role);
      });
    }

    if (options.isBlocked !== undefined)
      params = params.set('isBlocked', options.isBlocked.toString());

    if (options.startDate !== undefined)
      params = params.set('startDate', options.startDate.toISOString());

    if (options.endDate !== undefined)
      params = params.set('endDate', options.endDate.toISOString());

    return params;
  }

  getUsersInRoleWithName(role: AvailableUserRole, partialName: string) {
    let params = this.createHttpParams({
      filterRoles: [role],
      partialUserName: partialName,
    });

    return this._httpClient
      .get<PagedResult<User>>(this._apiUrl, {
        params: params,
      })
      .pipe(map((res) => res.items));
  }

  deleteUsers(ids: number[]) {
    return this._httpClient
      .delete(this._apiUrl, {
        body: ids,
      })
      .pipe(take(1));
  }

  changeBlockStatus(ids: number[], isBlocked: boolean) {
    return this._httpClient
      .patch(this._apiUrl, ids, {
        params: {
          isBlocked: isBlocked,
        },
      })
      .pipe(take(1));
  }

  modifyUserRoles(id: string, newRoles: AvailableUserRole[]) {
    return this._httpClient
      .patch(`${this._apiUrl}/${id}/roles`, newRoles)
      .pipe(take(1));
  }

  getUsersMetrics() {
    return this._httpClient.get<UsersMetrics>(this._apiUrl + '/metrics');
  }

  updateUser(user: UserUpdateDTO) {
    return this._httpClient.put<User>(this._apiUrl, user).pipe(take(1));
  }
}
