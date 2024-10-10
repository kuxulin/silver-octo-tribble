import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, take } from 'rxjs';
import { environment } from '../../environments/environment';
import UserQueryOptions from '../models/QueryOptions/UserQueryOptions';
import PagedResult from '../models/PagedResult';
import User from '../models/User';
import AvailableUserRole from '../models/enums/AvailableUserRole';
import UsersMetrics from '../models/UserMetrics';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _apiUrl = environment.api + '/User';

  constructor(private httpClient: HttpClient) {}

  getAllUsers(options: UserQueryOptions): Observable<PagedResult<User>> {
    let params = this.createHttpParams(options);
    return this.httpClient
      .get<PagedResult<User>>(this._apiUrl, {
        params: params,
      })
      .pipe(
        map((result) => {
          result.items.forEach((user: any) => {
            user.creationDate = new Date(user.creationDate.toString() + 'Z');
            user.roles = user.roleIds;
          });
          return result;
        })
      );
  }

  private createHttpParams(options: UserQueryOptions): HttpParams {
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

  deleteUsers(ids: string[]) {
    return this.httpClient
      .delete(this._apiUrl, {
        body: ids,
      })
      .pipe(take(1));
  }

  changeBlockStatus(ids: string[], isBlocked: boolean) {
    return this.httpClient
      .patch(this._apiUrl, ids, {
        params: {
          isBlocked: isBlocked,
        },
      })
      .pipe(take(1));
  }

  modifyUserRoles(id: string, newRoles: AvailableUserRole[]) {
    return this.httpClient
      .patch(`${this._apiUrl}/${id}/roles`, newRoles)
      .pipe(take(1));
  }

  getUsersMetrics() {
    return this.httpClient.get<UsersMetrics>(this._apiUrl + '/metrics');
  }
}
