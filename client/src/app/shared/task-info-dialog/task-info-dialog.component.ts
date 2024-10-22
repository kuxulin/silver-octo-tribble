import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCommonModule } from '@angular/material/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import TodoTask from '../models/TodoTask';
import User from '../models/User';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import AvailableTaskStatus from '../models/enums/AvailableStatus';
import { SelectListComponent } from '../select-list/select-list.component';
import AvailableUserRole from '../models/enums/AvailableUserRole';

@Component({
  selector: 'app-task-info-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatCommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    SelectListComponent,
  ],
  templateUrl: './task-info-dialog.component.html',
  styleUrl: './task-info-dialog.component.scss',
})
export class TaskInfoDialogComponent {
  task!: TodoTask;
  employee = AvailableUserRole.Employee;
  newUser: User | undefined;
  isManager = true;
  form = new FormGroup({
    title: new FormControl('', Validators.required),
    text: new FormControl('', Validators.required),
    status: new FormControl(AvailableTaskStatus[AvailableTaskStatus.Todo]),
  });

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data:
      | { task: TodoTask; user: User; isManager: boolean; employees: User[] }
      | undefined,
    public dialogRef: MatDialogRef<TaskInfoDialogComponent>
  ) {}

  ngOnInit() {
    if (!!this.data) {
      this.form.setControl('title', new FormControl(this.data.task.title));

      this.form.setControl('text', new FormControl(this.data.task.title));

      this.form.setControl(
        'status',
        new FormControl(AvailableTaskStatus[this.data.task.status])
      );

      this.isManager = this.data.isManager;
      if (!this.isManager) {
        this.form.controls.text.disable();
        this.form.controls.title.disable();
      }
    }
  }

  showAvailableTaskStatuses() {
    let statuses: string[] = [];
    statuses.push(AvailableTaskStatus[AvailableTaskStatus.Todo]);
    statuses.push(AvailableTaskStatus[AvailableTaskStatus.InProgress]);
    statuses.push(AvailableTaskStatus[AvailableTaskStatus.OnReview]);

    if (this.isManager)
      statuses.push(AvailableTaskStatus[AvailableTaskStatus.Completed]);

    return statuses;
  }

  onUserChanged(user: User) {
    this.newUser = user;
  }

  isSomethingChanged() {
    return (
      this.form.dirty ||
      (this.data?.user?.id !== undefined && this.newUser?.id === undefined) ||
      this.data?.user?.id !== this.newUser?.id
    );
  }

  isEverythingOk() {
    return this.form.valid;
  }

  onClose(flag: boolean) {
    if (!this.isSomethingChanged()) flag = false;

    let newTask: Partial<TodoTask> = {
      id: this.data?.task.id || undefined,
      title: this.form.value.title!,
      text: this.form.value.text!,
      status:
        AvailableTaskStatus[
          this.form.value.status! as keyof typeof AvailableTaskStatus
        ],
      projectId: '',
      employeeId: this.newUser?.employeeId!,
    };
    this.dialogRef.close({ flag, newTask });
  }
}
