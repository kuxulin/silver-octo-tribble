import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, shareReplay, take, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { SESSION_STORAGE } from '../../consts';
import LoginRegisterDTO from '../models/DTOs/LoginRegisterDTO';
import AvailableUserRole from '../models/enums/AvailableUserRole';
import { Router } from '@angular/router';
import UserAuthDTO from '../models/DTOs/UserAuthDTO';
import { OnlineUsersService } from './online-users.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _apiUrl = environment.server + 'api/auth';
  private _subject = new ReplaySubject<Object>(1);
  user$ = this._subject.asObservable() as Observable<UserAuthDTO>;

  constructor(
    private _httpClient: HttpClient,
    private _router: Router,
    private _onlineUsersService: OnlineUsersService
  ) {}

  login(userName: string, password: string) {
    return this._httpClient
      .post<UserAuthDTO>(
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
      .post<UserAuthDTO>(
        this._apiUrl + '/register',
        {
          ...dto,
        },
        { withCredentials: true }
      )
      .pipe(
        tap((res) => this.setSession(res)),
        shareReplay()
      );
  }

  private async makeUserOnline() {
    await this._onlineUsersService.makeUserOnline();
  }

  private makeUserOffline() {
    this._onlineUsersService.makeUserOffline();
  }

  private setSession(result: UserAuthDTO | null = null) {
    if (result) {
      sessionStorage.setItem(SESSION_STORAGE.TOKEN, result.token);

      let user: UserAuthDTO = {
        ...result,
        token: '',
      };

      this._subject.next(user);
      this.makeUserOnline();
    } else {
      this._subject.next(0);
      this._router.navigate(['login']);
      this.makeUserOffline();
    }
  }

  refreshTokens() {
    return this._httpClient
      .get<UserAuthDTO>(this._apiUrl + '/refresh', { withCredentials: true })
      .pipe(
        take(1),
        tap((res) => this.setSession(res)),
        shareReplay()
      );
  }

  getAuthToken() {
    return sessionStorage.getItem(SESSION_STORAGE.TOKEN);
  }

  isEmployeeInGeneral(user: UserAuthDTO) {
    return user.roles.some(
      (role) =>
        role.toString() ===
        AvailableUserRole[AvailableUserRole.Employee].toString()
    );
  }

  isManagerInGeneral(user: UserAuthDTO) {
    return user.roles.some(
      (role) =>
        role.toString() ===
        AvailableUserRole[AvailableUserRole.Manager].toString()
    );
  }

  isAdmin(user: UserAuthDTO) {
    return user.roles.some(
      (role) =>
        role.toString() ===
        AvailableUserRole[AvailableUserRole.Admin].toString()
    );
  }

  logOut() {
    sessionStorage.removeItem(SESSION_STORAGE.TOKEN);
    return this._httpClient
      .delete(this._apiUrl + '/logout', { withCredentials: true })
      .pipe(
        take(1),
        tap(() => this.setSession())
      );
  }
}
