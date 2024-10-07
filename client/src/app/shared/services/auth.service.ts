import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, shareReplay, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import AuthDTO from '../models/DTOs/AuthDTO';
import { SESSION_STORAGE } from '../../consts';
import LoginRegisterDTO from '../models/DTOs/RegisterDTO';
import UserAuthDTO from '../models/DTOs/UserAuthDTO';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.api + '/auth';
  private subject = new BehaviorSubject<UserAuthDTO | null>(null);
  user$ = this.subject.asObservable();

  constructor(private httpClient: HttpClient) {}

  login(userName: string, password: string) {
    return this.httpClient
      .post<AuthDTO>(
        this.apiUrl + '/login',
        {
          userName,
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
    return this.httpClient
      .post<AuthDTO>(this.apiUrl + '/register', {
        ...dto,
      })
      .pipe(
        tap((res) => this.setSession(res)),
        shareReplay()
      );
  }

  private setSession(result: AuthDTO) {
    sessionStorage.setItem(SESSION_STORAGE.TOKEN, result.token);
    let user: UserAuthDTO = {
      userName: result.userName,
      roles: result.roles,
    };
    this.subject.next(user);
  }

  logOut() {
    sessionStorage.removeItem(SESSION_STORAGE.TOKEN);
    this.subject.next(null);
  }
}
