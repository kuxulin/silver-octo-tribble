import { Component, Input } from '@angular/core';
import LoginRegisterDTO from '../models/DTOs/LoginRegisterDTO';
import { AuthService } from '../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { merge, of, Subject, takeUntil } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-auth-handler',
  standalone: true,
  imports: [MatButtonModule, RouterModule, CommonModule],
  templateUrl: './auth-handler.component.html',
  styleUrl: './auth-handler.component.scss',
})
export class AuthHandlerComponent {
  @Input({ required: true })
  areFieldsValid = false;
  @Input({ required: true })
  dto!: Partial<LoginRegisterDTO>;
  @Input({ required: true })
  isLogin = true;
  private _destroy$ = new Subject<boolean>();
  errorText = '';
  constructor(
    private _authService: AuthService,
    private _router: Router,
    private _dataService: DataService
  ) {}

  onSubmitClick() {
    if (!this.areFieldsValid) return;

    let methodToExecute$ = this.isLogin
      ? this._authService.login(this.dto.userName!, this.dto.password!)
      : this._authService.register(this.dto as LoginRegisterDTO);

    let logOut$ = !!this._dataService.getAuthToken()
      ? this._authService.logOut()
      : of();

    let combinedOperation$ = merge(logOut$, methodToExecute$);
    combinedOperation$.pipe(takeUntil(this._destroy$)).subscribe({
      next: async (res) => {
        if (res) {
          if (!this.isLogin) await new Promise((f) => setTimeout(f, 5000));

          this._router.navigate(['user/', res.id]);
        }
      },
      error: (response: HttpErrorResponse) => {
        this.errorText = !!response.error.message
          ? response.error.message
          : 'Some troubles with server.';
      },
    });
  }

  ngOnDestroy() {
    this._destroy$.next(true);
    this._destroy$.complete();
  }
}
