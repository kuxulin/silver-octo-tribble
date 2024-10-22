import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Observable, Subject, take } from 'rxjs';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { UserService } from '../../shared/services/user.service';
import User from '../../shared/models/User';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { ProjectService } from '../../shared/services/project.service';
import AvailableUserRole from '../../shared/models/enums/AvailableUserRole';
import { SelectListComponent } from '../../shared/select-list/select-list.component';

@Component({
  selector: 'app-create-project-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatSelectModule,
    CommonModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    SelectListComponent,
  ],
  templateUrl: './create-project-dialog.component.html',
  styleUrl: './create-project-dialog.component.scss',
})
export class CreateProjectDialogComponent implements OnInit, OnDestroy {
  managerCode = AvailableUserRole.Manager;
  employeeCode = AvailableUserRole.Employee;
  errorText = '';
  selectedManagers: User[] = [];
  selectedEmployees: User[] = [];
  private _destroy$ = new Subject<boolean>();
  creator$!: Observable<User>;
  form = new FormGroup({
    name: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
  });

  constructor(
    public dialogRef: MatDialogRef<CreateProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { managerId: number },
    private _userService: UserService,
    private _projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.creator$ = this._userService.getUserById(this.data.managerId);
  }

  isAnyFieldEmpty() {
    return (
      !this.form.value.name ||
      !this.form.value.description ||
      this.selectedManagers.length === 0 ||
      this.selectedEmployees.length === 0
    );
  }

  createProject() {
    if (this.isAnyFieldEmpty()) return;

    let dto = {
      name: this.form.value.name!,
      description: this.form.value.description!,
      managerIds: this.selectedManagers.map((m) => m.managerId!),
      employeeIds: this.selectedEmployees.map((m) => m.employeeId!),
    };

    this._projectService
      .createProject(dto)
      .pipe(take(1))
      .subscribe({
        next: (res) => this.onClose(true, res.id),
        error: (err) => (this.errorText = err.error),
      });
  }

  changeManagers(users: User[]) {
    this.selectedManagers = users;
  }

  changeEmployees(users: User[]) {
    this.selectedEmployees = users;
  }

  onClose(flag: boolean, id: string | undefined = undefined) {
    this.dialogRef.close({ flag, id });
  }

  ngOnDestroy() {
    this._destroy$.next(true);
    this._destroy$.complete();
  }
}
