import { Component, Inject, OnInit } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import AvailableUserRole from '../models/enums/AvailableUserRole';

@Component({
  selector: 'app-roles-assign-dialog',
  standalone: true,
  imports: [MatDialogModule, CommonModule, MatCheckboxModule, MatButtonModule],
  templateUrl: './roles-assign-dialog.component.html',
  styleUrl: './roles-assign-dialog.component.scss',
})
export class RolesAssignDialogComponent implements OnInit {
  newRoles: AvailableUserRole[] = [];
  availableUserRoles: string[] = Object.keys(AvailableUserRole).filter((key) =>
    isNaN(Number(key))
  );

  constructor(
    public dialogRef: MatDialogRef<RolesAssignDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { userName: string; userRoles: AvailableUserRole[] }
  ) {}

  ngOnInit(): void {
    this.newRoles = [...this.data.userRoles];
  }

  isAssigned(role: AvailableUserRole) {
    return this.newRoles.some((r) => r === role);
  }

  changeRolePeek(role: AvailableUserRole) {
    console.log(role);
    if (this.newRoles.some((r) => r === role))
      this.newRoles.splice(this.newRoles.indexOf(role), 1);
    else this.newRoles.push(role);
  }

  onClose(flag: boolean) {
    this.dialogRef.close({ flag, newRoles: this.newRoles });
  }
}
