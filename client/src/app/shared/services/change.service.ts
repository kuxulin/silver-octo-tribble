import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Change from '../models/Change';
import { formatDate } from '@angular/common';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChangeService {
  private _apiUrl = environment.api + '/Change';

  constructor(private _httpClient: HttpClient) {}

  convertChange(change: Change) {
    let ending = change.actionType.endsWith('e') ? 'd' : 'ed';
    let title = !!change.task ? change.task.title : change.taskTitle!;
    let action = change.actionType + ending;

    if (change.actionType.split(' ').length > 1)
      action = change.actionType.replace(' ', 'd ');

    let res = `${change.creator.userName} ${action} ${title} at ${formatDate(
      change.creationDate,
      'd.MM.yyy, H:mm',
      'en-US'
    )}`;

    return res;
  }

  getChangesFromProject(projectId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/project/' + projectId)
      .pipe(
        map((changes) =>
          changes.sort((a, b) => {
            let r =
              new Date(b.creationDate).getTime() -
              new Date(a.creationDate).getTime();
            return r;
          })
        )
      );
  }

  getChangesFromManager(managerId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/manager/' + managerId)
      .pipe(
        map((changes) =>
          changes.sort((a, b) => {
            let r =
              new Date(b.creationDate).getTime() -
              new Date(a.creationDate).getTime();
            return r;
          })
        )
      );
  }

  getChangesFromEmployee(employeeId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/employee/' + employeeId)
      .pipe(
        map((changes) =>
          changes.sort((a, b) => {
            let r =
              new Date(b.creationDate).getTime() -
              new Date(a.creationDate).getTime();
            return r;
          })
        )
      );
  }
}
