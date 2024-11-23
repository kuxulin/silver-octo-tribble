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
import { ImageService } from '../../shared/services/image.service';
import { DataService } from '../../shared/services/data.service';

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
      type: '',
    },
  };
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  checkedPassword = '';

  constructor(
    private _imageService: ImageService,
    private _dataService: DataService
  ) {}

  onImageClick() {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event) {
    let input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      let file = input.files[0];
      let reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = () => {
        let base64String = (reader.result as string).split(',')[1];

        this.dto.image = {
          name: file.name,
          content: base64String,
          type: file.type,
        };
      };
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

  getAppropriateImageSource(content: string, type: string) {
    return this._imageService.getAppropriateImageSource(content, type);
  }

  isPhoneValid(): boolean {
    return !this.dto.phoneNumber || isValidNumber(this.dto.phoneNumber);
  }
}
