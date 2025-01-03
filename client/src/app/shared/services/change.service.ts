import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Change from '../models/Change';
import { formatDate } from '@angular/common';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChangeService {
  private _apiUrl = environment.server + 'api/Change';

  constructor(private _httpClient: HttpClient) {}

  convertChange(change: Change) {
    let ending = change.actionType.endsWith('e') ? 'd' : 'ed';
    let title = !!change.task ? change.task.title : change.taskTitle!;
    let action = change.actionType + ending;

    if (change.actionType.split(' ').length > 1)
      action = change.actionType.replace(' ', 'd ');

    let timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    let res = `${change.creator.userName} ${action} ${title} at ${formatDate(
      change.creationDate,
      'd.MM.yyy, H:mm',
      'en-US',
      timezone
    )}`;

    return res;
  }

  getChangesFromProject(projectId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/project/' + projectId)
      .pipe(map((changes) => this.mapChanges(changes)));
  }

  getChangesFromManager(managerId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/manager/' + managerId)
      .pipe(map((changes) => this.mapChanges(changes)));
  }

  getChangesFromEmployee(employeeId: string) {
    return this._httpClient
      .get<Change[]>(this._apiUrl + '/employee/' + employeeId)
      .pipe(map((changes) => this.mapChanges(changes)));
  }

  private mapChanges(changes: Change[]) {
    changes.sort((a, b) => {
      let r =
        new Date(b.creationDate).getTime() - new Date(a.creationDate).getTime();
      return r;
    });

    changes.forEach((change) => {
      change.creationDate = new Date(change.creationDate.toString() + 'Z');
      return change;
    });

    return changes;
  }

  makeChangeRead(changeIds: string[]) {
    return this._httpClient.patch(this._apiUrl + '/read', changeIds);
  }
}
