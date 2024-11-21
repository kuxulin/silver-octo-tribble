import { Injectable } from '@angular/core';
import { SESSION_STORAGE } from '../../consts';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private _rowData: any;

  setRowData(data: any) {
    this._rowData = data;
  }

  getRowData() {
    return this._rowData;
  }

  getAuthToken() {
    return sessionStorage.getItem(SESSION_STORAGE.TOKEN);
  }

  setAuthToken(token: string) {
    if (!token) sessionStorage.removeItem(SESSION_STORAGE.TOKEN);
    else sessionStorage.setItem(SESSION_STORAGE.TOKEN, token);
  }

  convertArrayBufferToBase64String(array: ArrayBuffer) {
    var binary = '';
    var bytes = new Uint8Array(array);
    var len = bytes.byteLength;

    for (var i = 0; i < len; i++) binary += String.fromCharCode(bytes[i]);

    return window.btoa(binary);
  }
}
