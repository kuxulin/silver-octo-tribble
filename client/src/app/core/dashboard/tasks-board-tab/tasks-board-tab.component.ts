import { Component, EventEmitter, Input, Output } from '@angular/core';
import AvailableTaskStatus from '../../../shared/models/enums/AvailableStatus';
import {
  CdkDropList,
  CdkDrag,
  CdkDragDrop,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import TodoTask from '../../../shared/models/TodoTask';
import { TodoTaskService } from '../../../shared/services/todo-task.service';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'dashboard-tasks-board-tab',
  standalone: true,
  imports: [CdkDropList, CdkDrag, MatCardModule, CommonModule],
  templateUrl: './tasks-board-tab.component.html',
  styleUrl: './tasks-board-tab.component.scss',
})
export class TasksBoardTabComponent {
  taskStatuses = Object.values(AvailableTaskStatus).filter(
    (value) => typeof value === 'number'
  );

  @Input({ required: true })
  isManager!: boolean;
  @Input({ required: true })
  todo!: TodoTask[];
  @Input({ required: true })
  inProgress!: TodoTask[];
  @Input({ required: true })
  onReview!: TodoTask[];
  @Input({ required: true })
  completed!: TodoTask[];
  @Output()
  onTaskChanged = new EventEmitter();
  @Output()
  onTaskOpened = new EventEmitter<TodoTask>();
  getAppropriateTaskGroup(status: AvailableTaskStatus) {
    switch (status) {
      case AvailableTaskStatus.Todo:
        return this.todo;
      case AvailableTaskStatus.InProgress:
        return this.inProgress;
      case AvailableTaskStatus.OnReview:
        return this.onReview;
      case AvailableTaskStatus.Completed:
        return this.completed;
      default:
        return [];
    }
  }

  constructor(private _todoTaskService: TodoTaskService) {}

  getValueFromTaskStatusCode(code: number) {
    return AvailableTaskStatus[code];
  }

  dropTaskToNewStatus(
    event: CdkDragDrop<TodoTask[]>,
    newStatus: AvailableTaskStatus
  ) {
    if (event.previousContainer.id !== event.container.id) {
      const task = event.previousContainer.data[
        event.previousIndex
      ] as TodoTask;

      task.status = newStatus;

      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      this._todoTaskService
        .changeTaskStatus(task.id, task.status)
        .subscribe(() => this.onTaskChanged.emit());
    }
  }
}
