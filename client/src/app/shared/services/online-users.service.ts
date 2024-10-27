import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { SESSION_STORAGE } from '../../consts';
@Injectable({
  providedIn: 'root',
})
export class OnlineUsersService {
  private _hubConnection: HubConnection | null = null;
  private _onlineUsersSubject = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this._onlineUsersSubject.asObservable();
  constructor() {}

  private getAuthToken() {
    return sessionStorage.getItem(SESSION_STORAGE.TOKEN);
  }

  async startConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.server + 'onlineStatusHub', {
        accessTokenFactory: () => this.getAuthToken()!,
      })
      .build();

    await this._hubConnection.start();

    this._hubConnection.on('AddOnlineUser', (users: string[]) =>
      this._onlineUsersSubject.next(users)
    );

    this._hubConnection.on('RemoveOfflineUser', (users: string[]) =>
      this._onlineUsersSubject.next(users)
    );
  }

  async makeUserOnline() {
    await this.startConnection();
    this._hubConnection!.invoke('UserConnectedAsync');
  }

  makeUserOffline() {
    this._hubConnection!.invoke('UserDisconnectedAsync');
    this.stopConnection();
  }

  stopConnection() {
    this._hubConnection!.stop();
    this._hubConnection = null;
  }
}
