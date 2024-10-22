import { Component, inject, OnInit } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { ProjectService } from '../../shared/services/project.service';
import { map, merge, Observable, of, shareReplay, take, tap } from 'rxjs';
import Project from '../../shared/models/Project';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { CreateProjectDialogComponent } from '../create-project-dialog/create-project-dialog.component';
import { AuthService } from '../../shared/services/auth.service';
import UserAuthDTO from '../../shared/models/DTOs/UserAuthDTO';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionSelectionChange } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import TodoTask from '../../shared/models/TodoTask';
import { TodoTaskService } from '../../shared/services/todo-task.service';
import { TaskInfoDialogComponent } from '../../shared/task-info-dialog/task-info-dialog.component';
import { MatTableModule } from '@angular/material/table';
import {
  CdkDrag,
  CdkDragDrop,
  CdkDropList,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import AvailableTaskStatus from '../../shared/models/enums/AvailableStatus';
import { ProjectDetailsTabComponent } from './project-details-tab/project-details-tab.component';
import { TasksTableTabComponent } from './tasks-table-tab/tasks-table-tab.component';
import { TasksBoardTabComponent } from './tasks-board-tab/tasks-board-tab.component';
import Employee from '../../shared/models/Employee';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatTabsModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatSelectModule,
    MatCardModule,
    MatListModule,
    MatTableModule,
    CdkDropList,
    CdkDrag,
    ProjectDetailsTabComponent,
    TasksTableTabComponent,
    TasksBoardTabComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  projects$!: Observable<Project[]>;
  todoTasks$!: Observable<TodoTask[]>;
  currentUser$!: Observable<UserAuthDTO>;
  currentUserId!: number;
  selectedProjectId = '';
  isManager = false;
  isEmployee = false;
  currentProject$!: Observable<Project>;
  showButton = false;
  readonly dialog = inject(MatDialog);
  todo$!: Observable<TodoTask[]>;
  inProgress$!: Observable<TodoTask[]>;
  onReview$!: Observable<TodoTask[]>;
  completed$!: Observable<TodoTask[]>;

  constructor(
    private _projectService: ProjectService,
    private _authService: AuthService,
    private _todoTaskService: TodoTaskService
  ) {}

  ngOnInit(): void {
    this.currentUser$ = this._authService.user$;

    this.currentUser$.pipe(take(1)).subscribe((res) => {
      this.currentUserId = res.id;
      this.isManager = this._authService.isManager(res);
      this.isEmployee = this._authService.isEmployee(res);
      this.fetchProjects();
    });
  }

  fetchProjects() {
    let managerProjects$ = this._projectService.getProjectsByManager(
      this.currentUserId
    );
    let employeeProjects$ = this._projectService.getProjectsByEmployee(
      this.currentUserId
    );

    this.projects$ = merge(managerProjects$, employeeProjects$).pipe(
      tap((res) => this.chooseSelectedProject(res))
    );
  }

  chooseSelectedProject(projects: Project[]) {
    if (projects.length === 0) return;

    let selectedProject = projects[0];
    this.selectedProjectId = projects[0].id;

    if (this.selectedProjectId) {
      let res = projects.find((p) => p.id === this.selectedProjectId)!;
      this.selectedProjectId = res.id;
      selectedProject = res;
    }

    this.fetchCurrentProject();
    return selectedProject;
  }

  fetchCurrentProject() {
    this.currentProject$ = this._projectService.getProjectById(
      this.selectedProjectId
    );
    this.fetchTasks();
  }

  changeCurrentProject($event: MatOptionSelectionChange<Project>) {
    if (!$event.isUserInput) return;

    this.selectedProjectId = $event.source.value.id;
    this.fetchCurrentProject();
  }

  fetchTasks() {
    this.todoTasks$ = this._todoTaskService
      .getTasksByProjectId(this.selectedProjectId)
      .pipe(
        shareReplay(),
        map((res) => {
          if (this.isManager) return res;

          return res.filter(
            (task) => task.employee?.user.id === this.currentUserId
          );
        })
      );

    this.todo$ = this.todoTasks$.pipe(
      map((tasks) => tasks.filter((t) => t.status === AvailableTaskStatus.Todo))
    );

    this.inProgress$ = this.todoTasks$.pipe(
      map((tasks) =>
        tasks.filter((t) => t.status === AvailableTaskStatus.InProgress)
      )
    );

    this.onReview$ = this.todoTasks$.pipe(
      map((tasks) =>
        tasks.filter((t) => t.status === AvailableTaskStatus.OnReview)
      )
    );

    this.completed$ = this.todoTasks$.pipe(
      map((tasks) =>
        tasks.filter((t) => t.status === AvailableTaskStatus.Completed)
      )
    );
  }

  triggerCreateProjectDialog(id: number) {
    let dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      data: { managerId: id },
      width: '25%',
      height: '70%',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result && result.flag) {
        this.fetchProjects();
        this.selectedProjectId = result.id;
      }
    });
  }

  triggerCreateTaskDialog() {
    let dialogRef = this.dialog.open(TaskInfoDialogComponent, {
      width: '25%',
      height: '70%',
    });

    dialogRef
      .afterClosed()
      .subscribe((res: { flag: boolean; newTask: Partial<TodoTask> }) => {
        if (res && res.flag) {
          this._todoTaskService
            .createTask({
              title: res.newTask.title!,
              text: res.newTask.text!,
              projectId: this.selectedProjectId,
              employeeId: res.newTask.employee?.id!,
            })
            .subscribe(() => this.fetchTasks());
        }
      });
  }

  triggerOpenTaskInfoDialog(task: TodoTask, employees: Employee[]) {
    let dialogRef = this.dialog.open(TaskInfoDialogComponent, {
      data: {
        task,
        user: task.employee?.user,
        isManager: this.isManager,
        employees: employees.map((e) => e.user),
      },
      width: '25%',
      height: '70%',
    });

    dialogRef
      .afterClosed()
      .subscribe((res: { flag: boolean; newTask: Partial<TodoTask> }) => {
        if (res && res.flag) {
          this._todoTaskService
            .updateTask({
              id: res.newTask.id!,
              title: res.newTask.title!,
              text: res.newTask.text!,
              employeeId: res.newTask.employeeId!,
              status: res.newTask.status!,
            })
            .subscribe(() => this.fetchTasks());
        }
      });
  }
}
