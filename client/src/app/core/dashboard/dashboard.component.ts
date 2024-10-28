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
import TodoTask from '../../shared/models/TodoTask';
import { TodoTaskService } from '../../shared/services/todo-task.service';
import { TaskInfoDialogComponent } from '../../shared/task-info-dialog/task-info-dialog.component';
import AvailableTaskStatus from '../../shared/models/enums/AvailableStatus';
import { ProjectDetailsTabComponent } from './project-details-tab/project-details-tab.component';
import { TasksTableTabComponent } from './tasks-table-tab/tasks-table-tab.component';
import { TasksBoardTabComponent } from './tasks-board-tab/tasks-board-tab.component';
import Employee from '../../shared/models/Employee';
import { LogsDialogComponent } from '../../shared/logs-dialog/logs-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatTabsModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatSelectModule,
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
  currentUser!: UserAuthDTO;
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
    private _todoTaskService: TodoTaskService,
    private _route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this._route.queryParams.subscribe(
      (params) => (this.selectedProjectId = params['selectedProjectId'])
    );

    this.currentUser$ = this._authService.user$.pipe(
      tap((res) => {
        this.currentUser = res;
        this.isManager = this._authService.isManagerInGeneral(res);
        this.isEmployee = this._authService.isEmployeeInGeneral(res);
        this.fetchProjects();
      })
    );
  }

  fetchProjects() {
    let managerProjects$ = !!this.currentUser.managerId
      ? this._projectService.getProjectsByManager(this.currentUser.managerId)
      : of([]);
    let employeeProjects$ = !!this.currentUser.employeeId
      ? this._projectService.getProjectsByEmployee(this.currentUser.employeeId)
      : of([]);

    this.projects$ = merge(managerProjects$, employeeProjects$).pipe(
      tap((res) => this.chooseSelectedProject(res))
    );
  }

  chooseSelectedProject(projects: Project[]) {
    if (projects.length === 0) return;

    let selectedProject = projects[0];

    if (this.selectedProjectId) {
      let res = projects.find((p) => p.id === this.selectedProjectId)!;
      this.selectedProjectId = res.id;
      selectedProject = res;
    } else this.selectedProjectId = projects[0].id;

    this.fetchCurrentProject();
    return selectedProject;
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

  triggerOpenLogsFile(projectId: string) {
    let dialogRef = this.dialog.open(LogsDialogComponent, {
      data: { projectId },
      width: '40%',
      height: '70%',
    });
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
            (task) => task.employee?.user.id === this.currentUser.id
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

  triggerCreateTaskDialog(employees: Employee[]) {
    let dialogRef = this.dialog.open(TaskInfoDialogComponent, {
      data: { employees: employees.map((e) => e.user) },
      width: '25%',
      height: '55%',
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
      height: '55%',
    });

    dialogRef
      .afterClosed()
      .subscribe((res: { flag: boolean; newTask: TodoTask }) => {
        if (res && res.flag) {
          let request = this._todoTaskService.deleteTask(task.id);

          if (res.newTask)
            request = this.handlePossibleUpdates(task, res.newTask);

          request.subscribe(() => this.fetchTasks());
        }
      });
  }

  private handlePossibleUpdates(task: TodoTask, newTask: TodoTask) {
    let request = new Observable<Object>();
    if (this.isManager && task.employee?.id != newTask.employee?.id)
      request = this._todoTaskService.changeTaskEmployee(
        newTask.id,
        newTask.employee?.id!
      );

    if (
      this.isManager &&
      (task.title != newTask.title || task.text != newTask.text)
    )
      request = merge(
        request,
        this._todoTaskService.updateTask({ ...newTask })
      );

    if (task.status != newTask.status)
      request = merge(
        request,
        this._todoTaskService.changeTaskStatus(newTask.id, newTask.status)
      );

    return request;
  }
}
