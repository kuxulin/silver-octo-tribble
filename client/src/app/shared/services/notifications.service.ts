import { EventEmitter, inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { DataService } from './data.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import Change from '../models/Change';
import { ChangeService } from './change.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private _hubConnection: HubConnection | null = null;
  private _snackBar = inject(MatSnackBar);
  onChangeCreatedEvent = new EventEmitter();
  constructor(
    private _dataService: DataService,
    private _changeService: ChangeService
  ) {}

  async startConnectionAsync() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.server + 'notificationsHub', {
        accessTokenFactory: () => this._dataService.getAuthToken()!,
      })
      .build();

    await this._hubConnection.start();

    this._hubConnection.on('ChangeCreated', (change: Change) =>
      this.onChangeCreated(change)
    );
  }

  private onChangeCreated(change: Change) {
    this._snackBar.open(this._changeService.convertChange(change), 'Ok');
    this.onChangeCreatedEvent.emit();
  }

  stopConnection() {
    this._hubConnection?.stop();
    this._hubConnection = null;
  }
}
