import { Component, Input, OnInit } from '@angular/core';
import { OnlineUsersService } from '../services/online-users.service';
import { CommonModule } from '@angular/common';
import Image from '../models/Image';
import { ImageService } from '../services/image.service';
import { map, Observable } from 'rxjs';

@Component({
  selector: 'app-avatar-image',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avatar-image.component.html',
  styleUrl: './avatar-image.component.scss',
})
export class AvatarImageComponent implements OnInit {
  @Input({ required: true }) imageId = '';
  @Input({ required: true }) userName = '';
  image$!: Observable<Image>;
  constructor(
    private _onlineUsersService: OnlineUsersService,
    private _imageService: ImageService
  ) {}

  ngOnInit(): void {
    this.image$ = this._imageService.getThumbnailImage(this.imageId).pipe(
      map((image) => {
        image.content = this._imageService.getAppropriateImageSource(
          image.content,
          image.type
        );
        return image;
      })
    );
  }

  getOnlineUsers() {
    return this._onlineUsersService.onlineUsers$;
  }
}
