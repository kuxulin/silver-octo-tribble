import { Component, Input, OnInit } from '@angular/core';
import { OnlineUsersService } from '../services/online-users.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-avatar-image',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avatar-image.component.html',
  styleUrl: './avatar-image.component.scss',
})
export class AvatarImageComponent implements OnInit {
  @Input({ required: true }) imageContent = '';
  @Input({ required: true }) userName = '';
  constructor(private _onlineUsersService: OnlineUsersService) {}

  ngOnInit(): void {
    this.imageContent = 'data:image/jpeg;base64,' + this.imageContent;
  }

  getOnlineUsers() {
    return this._onlineUsersService.onlineUsers$;
  }
}
