import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import ProjectCreateDTO from '../models/DTOs/ProjectCreateDTO';
import Project from '../models/Project';
import { map, take } from 'rxjs';
import ProjectUpdateDTO from '../models/DTOs/ProjectUpdateDTO';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private _apiUrl = environment.api + '/Project';

  constructor(private _httpClient: HttpClient) {}

  getProjectsByManager(userId: number) {
    return this._httpClient.get<Project[]>(this._apiUrl + '/manager/' + userId);
  }

  getProjectsByEmployee(userId: number) {
    return this._httpClient.get<Project[]>(
      this._apiUrl + '/employee/' + userId
    );
  }

  getProjectById(id: string) {
    return this._httpClient.get<Project>(this._apiUrl + '/' + id);
  }

  createProject(dto: ProjectCreateDTO) {
    return this._httpClient.post<Project>(this._apiUrl, {
      ...dto,
    });
  }

  updateProject(dto: Partial<ProjectUpdateDTO>) {
    return this._httpClient
      .put(this._apiUrl, {
        ...dto,
      })
      .pipe(take(1));
  }
}
