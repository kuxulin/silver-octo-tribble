import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import Image from '../models/Image';
import { shareReplay } from 'rxjs';

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
    var urlPattern = new RegExp(
      '^(https?:\\/\\/)?' + // validate protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // validate domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // validate OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // validate port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // validate query string
        '(\\#[-a-z\\d_]*)?$',
      'i'
    );

    if (urlPattern.test(content)) return content;
    else return `data:${type};base64,` + content;
  }
}
