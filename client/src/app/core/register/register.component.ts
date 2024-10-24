import { Component, ElementRef, ViewChild } from '@angular/core';
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
    image: {
      content: '',
      name: '',
    },
  };

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  checkedPassword = '';

  onImageClick() {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event) {
    let input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      let file = input.files[0];
      let reader = new FileReader();

      reader.onload = () => {
        let base64String = reader.result as string;
        this.dto.image = {
          name: file.name,
          content: base64String.split(',')[1],
        };
      };

      reader.readAsDataURL(file);
    }
  }

  areFieldsValid(): boolean {
    return (
      !!this.dto.userName &&
      !!this.dto.password &&
      !!this.dto.firstName &&
      !!this.dto.lastName &&
      !!this.dto.phoneNumber &&
      !!this.dto.image.content &&
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
