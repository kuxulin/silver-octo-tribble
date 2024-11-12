import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import TodoTask from '../models/TodoTask';
import { take } from 'rxjs';
import TodoTaskCreateDTO from '../models/DTOs/TodoTaskCreateDTO';
import TodoTaskUpdateDTO from '../models/DTOs/TodoTaskUpdateDTO';
import AvailableTaskStatus from '../models/enums/AvailableStatus';

@Injectable({
  providedIn: 'root',
})
export class TodoTaskService {
  private _apiUrl = environment.server + 'api/TodoTask';

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

  changeTaskStatus(taskId: string, status: AvailableTaskStatus) {
    return this._httpClient
      .patch(
        this._apiUrl + '/status',
        {},
        {
          params: {
            taskId,
            status,
          },
        }
      )
      .pipe(take(1));
  }

  changeTaskEmployee(taskId: string, employeeId: string) {
    let params = new HttpParams();
    params = params.append('taskId', taskId);

    if (employeeId) params = params.append('employeeId', employeeId);

    return this._httpClient
      .patch(
        this._apiUrl + '/employee',
        {},
        {
          params: params,
        }
      )
      .pipe(take(1));
  }

  deleteTask(id: string) {
    return this._httpClient.delete(this._apiUrl + '/' + id).pipe(take(1));
  }
}
