import { Component, Inject } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { Observable } from 'rxjs';
import Change from '../models/Change';
import { ChangeService } from '../services/change.service';
import { CommonModule, formatDate } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { SortByDatePipe } from '../pipes/sort-by-date.pipe';

@Component({
  selector: 'app-logs-dialog',
  standalone: true,
  imports: [CommonModule, MatListModule, MatDialogModule, SortByDatePipe],
  templateUrl: './logs-dialog.component.html',
  styleUrl: './logs-dialog.component.scss',
})
export class LogsDialogComponent {
  changes$!: Observable<Change[]>;
  constructor(
    public dialogRef: MatDialogRef<LogsDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { projectId: string },
    private _changeService: ChangeService
  ) {}

  ngOnInit() {
    this.changes$ = this._changeService.getChangesFromProject(
      this.data.projectId
    );
  }

  convertChange(change: Change) {
    let ending = change.actionType.endsWith('e') ? 'd' : 'ed';
    let title = !!change.task ? change.task.title : change.taskTitle!;
    let res = `${change.creator.userName} ${
      change.actionType
    }${ending} ${title} at ${formatDate(
      change.creationDate,
      'd.MM.yyy, H:mm',
      'en-US'
    )}`;

    return res;
  }
}
