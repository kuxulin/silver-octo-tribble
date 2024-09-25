import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import AuthDTO from '../models/DTOs/AuthDTO';
import { SESSION_STORAGE } from '../../consts';
import RegisterDTO from '../models/DTOs/registerDTO';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.api + '/auth';
  constructor(private httpClient: HttpClient) {}

  login(username: string, password: string) {
    return this.httpClient
      .post<AuthDTO>(this.apiUrl + '/login', {
        username,
        password,
      })
      .pipe(tap((res) => this.setSession(res)));
  }

  register(dto: RegisterDTO) {
    return this.httpClient.post<AuthDTO>(this.apiUrl + '/register', {
      ...dto,
    });
  }
  private setSession(result: AuthDTO) {
    sessionStorage.setItem(SESSION_STORAGE.TOKEN, result.token);
  }
}
