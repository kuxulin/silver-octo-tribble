import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Image from '../models/Image';
import { delay, shareReplay } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private _apiUrl = environment.server + 'api/Image';

  constructor(private _httpClient: HttpClient) {}

  getOriginalImage(id: string) {
    return this._httpClient
      .get<Image>(this._apiUrl + `/original/${id}`)
      .pipe(shareReplay());
  }

  getProfileImage(id: string) {
    return this._httpClient
      .get<Image>(this._apiUrl + `/profile/${id}`)
      .pipe(shareReplay());
  }

  getThumbnailImage(id: string) {
    return this._httpClient
      .get<Image>(this._apiUrl + `/thumbnail/${id}`)
      .pipe(shareReplay());
  }

  getAppropriateImageSource(content: string, type: string) {
    let expression = new RegExp(
      /(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g
    );
    if (content.match(expression)) return content;
    else return `data:${type};base64,` + content;
  }
}
