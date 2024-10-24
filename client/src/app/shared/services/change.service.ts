import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Change from '../models/Change';

@Injectable({
  providedIn: 'root',
})
export class ChangeService {
  private _apiUrl = environment.api + '/Change';

  constructor(private _httpClient: HttpClient) {}

  getChangesFromProject(projectId: string) {
    return this._httpClient.get<Change[]>(
      this._apiUrl + '/project/' + projectId
    );
  }
}
