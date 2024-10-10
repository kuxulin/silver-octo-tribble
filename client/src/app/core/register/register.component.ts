import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import LoginRegisterDTO from '../../shared/models/DTOs/LoginRegisterDTO';
import { AuthHandlerComponent } from '../../shared/auth-handler/auth-handler.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { isPossiblePhoneNumber, isValidNumber } from 'libphonenumber-js';
import {
  ErrorStateMatcher,
  ShowOnDirtyErrorStateMatcher,
} from '@angular/material/core';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    RouterModule,
    AuthHandlerComponent,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher },
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  dto: LoginRegisterDTO = {
    userName: '',
    password: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
  };
  checkedPassword = '';

  areFieldsValid(): boolean {
    return (
      !!this.dto.userName &&
      !!this.dto.password &&
      !!this.dto.firstName &&
      !!this.dto.lastName &&
      !!this.dto.phoneNumber &&
      this.arePasswordsEqual() &&
      this.isPhoneValid()
    );
  }

  arePasswordsEqual(): boolean {
    return this.dto.password === this.checkedPassword;
  }

  isPhoneValid(): boolean {
    return !this.dto.phoneNumber || isValidNumber(this.dto.phoneNumber);
  }
}
