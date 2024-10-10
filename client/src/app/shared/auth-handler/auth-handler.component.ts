import { Component, Input } from '@angular/core';
import LoginRegisterDTO from '../models/DTOs/LoginRegisterDTO';
import { AuthService } from '../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Subject, takeUntil } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-auth-handler',
  standalone: true,
  imports: [MatButtonModule, RouterModule],
  templateUrl: './auth-handler.component.html',
  styleUrl: './auth-handler.component.scss',
})
export class AuthHandlerComponent {
  @Input({ required: true })
  areFieldsValid = false;
  @Input({ required: true })
  dto!: LoginRegisterDTO;
  @Input({ required: true })
  isLogin = true;
  private _destroy$ = new Subject<boolean>();
  errorText = '';

  constructor(private _authService: AuthService, private router: Router) {}

  onSubmitClick() {
    if (!this.areFieldsValid) return;

    let methodToExecute$ = this.isLogin
      ? this._authService.login(this.dto.userName, this.dto.password)
      : this._authService.register(this.dto);

    methodToExecute$.pipe(takeUntil(this._destroy$)).subscribe({
      next: () => {
        this.router.navigate(['/initial']);
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
