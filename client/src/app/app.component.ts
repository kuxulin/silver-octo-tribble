import { Component, HostListener, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderMenuComponent } from './core/header-menu/header-menu.component';
import { AuthService } from './shared/services/auth.service';
import { OnlineUsersService } from './shared/services/online-users.service';
import { DataService } from './shared/services/data.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderMenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'client';

  constructor(
    private _authService: AuthService,
    private _onlineUsersService: OnlineUsersService,
    private _dataService: DataService
  ) {}

  ngOnInit() {
    this._authService.refreshTokens().subscribe();
  }

  @HostListener('window:beforeunload')
  unloadHandler() {
    if (this._dataService.getAuthToken())
      this._onlineUsersService.makeUserOffline();
  }
}
