import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { FormsModule } from '@angular/forms';

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
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  hidePassword = signal(true);
  username = '';
  password = '';

  constructor(private authService: AuthService) {}

  onPasswordVisibilityChanged($event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    $event.stopPropagation();
  }

  areFieldsValid(): boolean {
    return !!this.username && !!this.password;
  }
  onSubmitClick() {
    if (!this.areFieldsValid()) return;

    this.authService
      .login(this.username, this.password)
      .subscribe({error:(err) => {
        console.log(err)
      }});
  }
}
