import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, shareReplay, take, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import AuthDTO from '../models/DTOs/AuthDTO';
import { SESSION_STORAGE } from '../../consts';
import LoginRegisterDTO from '../models/DTOs/RegisterDTO';
import User from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.api + '/auth';
  private subject = new BehaviorSubject<User | null>(null);
  user$ = this.subject.asObservable();

  constructor(private httpClient: HttpClient) {}

  login(username: string, password: string) {
    return this.httpClient
      .post<AuthDTO>(
        this.apiUrl + '/login',
        {
          username,
          password,
        },
        { withCredentials: true }
      )
      .pipe(
        tap((res) => this.setSession(res)),
        shareReplay()
      );
  }

  register(dto: LoginRegisterDTO) {
    return this.httpClient.post<AuthDTO>(this.apiUrl + '/register', {
      ...dto,
    });
  }

  private setSession(result: AuthDTO) {
    sessionStorage.setItem(SESSION_STORAGE.TOKEN, result.token);
    let user: User = {
      username: result.userName,
      roles: result.roles,
      id: undefined,
      fullName: undefined,
      phoneNumber: undefined,
    };
    this.subject.next(user);
  }

  logOut() {
    sessionStorage.removeItem(SESSION_STORAGE.TOKEN);
    this.subject.next(null);
  }
}
