import { HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { catchError, finalize, Observable, switchMap, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import AuthDTO from '../models/DTOs/AuthDTO';

let refreshingTokens: Observable<AuthDTO> | null = null;

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  if (
    ['auth/login', 'auth/register', 'auth/refresh'].some((url) =>
      req.url.includes(url)
    )
  )
    return next(req);

  const authService: AuthService = inject(AuthService);
  const router: Router = inject(Router);

  let token = authService.getAuthToken();
  let tokenInfo: JwtPayload;

  try {
    tokenInfo = jwtDecode(token!);
  } catch (error) {}

  if (token === null || tokenInfo!.exp! < Math.floor(Date.now() / 1000)) {
    if (!refreshingTokens)
      refreshingTokens = authService
        .refreshTokens()
        .pipe(finalize(() => (refreshingTokens = null)));

    return refreshingTokens.pipe(
      switchMap(() => {
        token = authService.getAuthToken();

        req = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${token}`),
        });

        return next(req);
      }),
      catchError((error) => {
        router.navigate(['login']);
        return throwError(() => new Error(error));
      })
    );
  }

  req = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`),
  });

  return next(req);
}
