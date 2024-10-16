import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, shareReplay, take, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import AuthDTO from '../models/DTOs/AuthDTO';
import { SESSION_STORAGE } from '../../consts';
import LoginRegisterDTO from '../models/DTOs/LoginRegisterDTO';
import UserAuthDTO from '../models/DTOs/UserAuthDTO';
import AvailableUserRole from '../models/enums/AvailableUserRole';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _apiUrl = environment.api + '/auth';
  private _subject = new ReplaySubject<Object>();
  user$ = this._subject.asObservable() as Observable<UserAuthDTO>;

  constructor(private _httpClient: HttpClient) {}

  login(userName: string, password: string) {
    return this._httpClient
      .post<AuthDTO>(
        this._apiUrl + '/login',
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
    return this._httpClient
      .post<AuthDTO>(this._apiUrl + '/register', {
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
      id: result.id,
    };
    this._subject.next(user);
  }

  refreshTokens() {
    return this._httpClient
      .get<AuthDTO>(this._apiUrl + '/refresh', { withCredentials: true })
      .pipe(
        take(1),
        tap((res) => this.setSession(res)),
        shareReplay()
      );
  }

  getAuthToken() {
    return sessionStorage.getItem(SESSION_STORAGE.TOKEN);
  }

  isEmployee(user: UserAuthDTO) {
    return user.roles.some(
      (role) => role === AvailableUserRole[AvailableUserRole.Employee]
    );
  }

  isManager(user: UserAuthDTO) {
    return user.roles.some(
      (role) => role === AvailableUserRole[AvailableUserRole.Manager]
    );
  }

  isAdmin(user: UserAuthDTO) {
    return user.roles.some(
      (role) => role === AvailableUserRole[AvailableUserRole.Admin]
    );
  }

  logOut() {
    sessionStorage.removeItem(SESSION_STORAGE.TOKEN);
    return this._httpClient
      .delete(this._apiUrl + '/logout', { withCredentials: true })
      .pipe(
        take(1),
        tap(() => this._subject.next(0))
      );
  }
}
