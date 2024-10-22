import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import TodoTask from '../../../shared/models/TodoTask';
import AvailableTaskStatus from '../../../shared/models/enums/AvailableStatus';

@Component({
  selector: 'dashboard-tasks-table-tab',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, MatTableModule],
  templateUrl: './tasks-table-tab.component.html',
  styleUrl: './tasks-table-tab.component.scss',
})
export class TasksTableTabComponent {
  showButton = false;
  @Input({ required: true })
  isManager!: boolean;
  @Input({ required: true })
  todoTasks!: TodoTask[];
  @Output()
  onTaskCreated = new EventEmitter();
  @Output()
  onTaskOpened = new EventEmitter<TodoTask>();

  getValueFromTaskStatusCode(code: number) {
    return AvailableTaskStatus[code];
  }
}
