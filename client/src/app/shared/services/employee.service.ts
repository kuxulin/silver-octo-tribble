import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import Employee from '../models/Employee';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private _apiUrl = environment.server + 'api/Employee';

  constructor(private _httpClient: HttpClient) {}

  getEmployeesByProjectId(projectId: string) {
    return this._httpClient.get<Employee[]>(this._apiUrl + '/' + projectId);
  }
}
