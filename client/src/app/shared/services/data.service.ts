import { Injectable } from '@angular/core';

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
}
