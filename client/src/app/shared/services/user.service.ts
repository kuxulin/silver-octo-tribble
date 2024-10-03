import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import User from '../models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.api + '/User';

  constructor(private httpClient: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.apiUrl).pipe(
      map((users) => {
        users.forEach((user) => {
          user.creationDate = new Date(user.creationDate.toString() + 'Z');
        });
        return users;
      })
    );
  }
}
