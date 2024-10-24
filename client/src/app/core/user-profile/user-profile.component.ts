import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { CommonModule } from '@angular/common';
import { Observable, Subject, takeUntil } from 'rxjs';
import User from '../../shared/models/User';
import UserAuthDTO from '../../shared/models/DTOs/UserAuthDTO';
import { UserService } from '../../shared/services/user.service';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import PhoneNumberValidator from '../../shared/validators/PhoneNumberValidator';
import AvailableUserRole from '../../shared/models/enums/AvailableUserRole';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    MatButtonModule,
    RouterModule,
    CommonModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss',
})
export class UserProfileComponent implements OnInit, OnDestroy {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  userAuth$!: Observable<UserAuthDTO>;
  user$!: Observable<User>;
  user!: User;
  private _destroy$ = new Subject<boolean>();
  isTheSameUserMarker = false;
  form = new FormGroup({
    userName: new FormControl('', Validators.minLength(3)),
    firstName: new FormControl('', Validators.minLength(3)),
    lastName: new FormControl('', Validators.minLength(3)),
    phoneNumber: new FormControl('', PhoneNumberValidator()),
    roles: new FormControl({ value: '', disabled: true }),
  });

  constructor(
    private _authService: AuthService,
    private _userService: UserService,
    private _route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.userAuth$ = this._authService.user$;
    let id = +this._route.snapshot.paramMap.get('id')!;
    this.user$ = this._userService.getUserById(id);

    this._userService
      .getUserById(id)
      .pipe(takeUntil(this._destroy$))
      .subscribe((res) => {
        this.user = res;
        this.form.patchValue({
          userName: this.user.userName,
          firstName: this.user.firstName,
          lastName: this.user.lastName,
          phoneNumber: this.user.phoneNumber,
          roles: this.user.roleIds
            ?.map((id) => AvailableUserRole[id])
            .join(', '),
        });
      });
  }

  isUserTheSame(authenticatedUserId: number, viewedUserId: number) {
    this.isTheSameUserMarker = authenticatedUserId === viewedUserId;
    return this.isTheSameUserMarker;
  }

  onImageClick() {
    if (this.isTheSameUserMarker) this.fileInput.nativeElement.click();
  }

  onFileSelected(event: Event) {
    let input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      let file = input.files[0];
      let reader = new FileReader();

      reader.onload = () => {
        let base64String = reader.result as string;
        this.user.image = {
          name: file.name,
          content: base64String.split(',')[1],
        };
      };

      reader.readAsDataURL(file);
    }
  }

  updateUser() {
    if (this.form.invalid || !this.isTheSameUserMarker) return;

    this._userService
      .updateUser({
        ...this.form.getRawValue(),
        id: this.user.id,
        imageDto: this.user.image,
      })
      .subscribe((res) => (this.user = res));
  }

  areActionsAllowed() {
    return this.isTheSameUserMarker && !this.form.invalid;
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
    this._destroy$.complete();
  }
}
