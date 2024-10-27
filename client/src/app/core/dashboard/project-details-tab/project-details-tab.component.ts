import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import Employee from '../../../shared/models/Employee';
import AvailableUserRole from '../../../shared/models/enums/AvailableUserRole';
import Manager from '../../../shared/models/Manager';
import Project from '../../../shared/models/Project';
import User from '../../../shared/models/User';
import { AuthService } from '../../../shared/services/auth.service';
import { ProjectService } from '../../../shared/services/project.service';
import { TodoTaskService } from '../../../shared/services/todo-task.service';
import { UpdateUsersInProjectDialogComponent } from '../../../shared/update-users-in-project-dialog/update-users-in-project-dialog.component';
import { AvatarImageComponent } from '../../../shared/avatar-image/avatar-image.component';

@Component({
  selector: 'dashboard-project-details-tab',
  standalone: true,
  imports: [
    MatListModule,
    MatIconModule,
    MatButtonModule,
    AvatarImageComponent,
  ],
  templateUrl: './project-details-tab.component.html',
  styleUrl: './project-details-tab.component.scss',
})
export class ProjectDetailsTabComponent {
  @Input({ required: true })
  currentProject!: Project;
  @Input({ required: true })
  isManager!: boolean;
  readonly dialog = inject(MatDialog);
  showButton = false;
  @Output()
  projectChanged = new EventEmitter();
  constructor(private _projectService: ProjectService) {}

  triggerChangeUsersInProject(
    entities: (Employee | Manager)[],
    projectId: string,
    isManager: boolean
  ) {
    let users = entities.map((e) => e.user);
    let dialogRef = this.dialog.open(UpdateUsersInProjectDialogComponent, {
      data: {
        initialUsers: users,
        userRole: isManager
          ? AvailableUserRole.Manager
          : AvailableUserRole.Employee,
      },
      width: '25%',
      height: '25%',
    });

    dialogRef
      .afterClosed()
      .subscribe((result: { flag: boolean; newUsers: User[] }) => {
        if (result && result.flag) {
          let managerIds = isManager
            ? result.newUsers.map((u) => u.managerId!)
            : undefined;
          let employeeIds = isManager
            ? undefined
            : result.newUsers.map((u) => u.employeeId!);

          this._projectService
            .updateProject({
              employeeIds,
              managerIds,
              id: projectId,
            })
            .subscribe(() => this.projectChanged.emit());
        }
      });
  }
}
