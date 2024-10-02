import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthHandlerComponent } from '../../shared/auth-handler/auth-handler.component';
import LoginRegisterDTO from '../../shared/models/DTOs/RegisterDTO';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    RouterModule,
    FormsModule,
    AuthHandlerComponent,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  hidePassword = signal(true);
  dto: LoginRegisterDTO = {
    username: '',
    password: '',
    fullName: '',
    phoneNumber: '',
  };

  onPasswordVisibilityChanged($event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    $event.stopPropagation();
  }

  areFieldsValid(): boolean {
    return !!this.dto.username && !!this.dto.password;
  }
}
