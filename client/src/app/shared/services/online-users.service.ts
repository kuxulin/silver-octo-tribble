import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DataService } from './data.service';
import { NotificationsService } from './notifications.service';
import { SESSION_STORAGE } from '../../consts';
@Injectable({
  providedIn: 'root',
})
export class OnlineUsersService {
  private _hubConnection: HubConnection | null = null;
  private _onlineUsersSubject = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this._onlineUsersSubject.asObservable();
  constructor(
    private _dataService: DataService,
    private _notificationsService: NotificationsService
  ) {}

  async startConnectionAsync() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.server + 'onlineStatusHub', {
        accessTokenFactory: () => this.getAuthToken()!,
      })
      .build();

    this._hubConnection.on('AddOnlineUser', (users: string[]) =>
      this._onlineUsersSubject.next(users)
    );

    this._hubConnection.on('RemoveOfflineUser', (users: string[]) =>
      this._onlineUsersSubject.next(users)
    );

    await this._hubConnection.start();
  }

  getAuthToken() {
    return sessionStorage.getItem(SESSION_STORAGE.TOKEN);
  }

  async makeUserOnline() {
    await this.startConnectionAsync();
    await this._notificationsService.startConnectionAsync();
  }

  makeUserOffline() {
    this.stopConnection();
  }

  stopConnection() {
    this._hubConnection!.stop();
    this._hubConnection = null;
  }
}
