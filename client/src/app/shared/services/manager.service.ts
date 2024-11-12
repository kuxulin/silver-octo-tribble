import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Manager from '../models/Manager';

@Injectable({
  providedIn: 'root',
})
export class ManagerService {
  private _apiUrl = environment.server + 'api/Manager';

  constructor(private _httpClient: HttpClient) {}

  getManagersByProjectId(projectId: string) {
    return this._httpClient.get<Manager[]>(this._apiUrl + '/' + projectId);
  }
}
