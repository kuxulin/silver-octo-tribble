import { Component, Inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import AvailableUserRole from '../models/enums/AvailableUserRole';
import User from '../models/User';
import { SelectListComponent } from '../select-list/select-list.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-update-users-in-project-dialog',
  standalone: true,
  imports: [SelectListComponent, MatDialogModule, MatButtonModule],
  templateUrl: './update-users-in-project-dialog.component.html',
  styleUrl: './update-users-in-project-dialog.component.scss',
})
export class UpdateUsersInProjectDialogComponent {
  newUsers: User[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: { initialUsers: User[]; userRole: AvailableUserRole },
    public dialogRef: MatDialogRef<UpdateUsersInProjectDialogComponent>
  ) {}

  ngOnInit() {}

  onOutputUpdate(users: User[]) {
    this.newUsers = users;
  }

  onClose(flag: boolean) {
    this.dialogRef.close({ flag, newUsers: this.newUsers });
  }
}
