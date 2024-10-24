import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import TodoTask from '../models/TodoTask';
import { take } from 'rxjs';
import TodoTaskCreateDTO from '../models/DTOs/TodoTaskCreateDTO';
import TodoTaskUpdateDTO from '../models/DTOs/TodoTaskUpdateDTO';

@Injectable({
  providedIn: 'root',
})
export class TodoTaskService {
  private _apiUrl = environment.api + '/TodoTask';

  constructor(private _httpClient: HttpClient) {}

  getTasksByProjectId(projectId: string) {
    return this._httpClient.get<TodoTask[]>(
      this._apiUrl + '/project/' + projectId
    );
  }

  createTask(task: TodoTaskCreateDTO) {
    return this._httpClient
      .post(this._apiUrl, {
        ...task,
      })
      .pipe(take(1));
  }

  updateTask(task: TodoTaskUpdateDTO) {
    return this._httpClient.put(this._apiUrl, { ...task }).pipe(take(1));
  }
}
