import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Statistic, Revenue } from '../models';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  getNewRegisters(key , dateFrom, dateTo) {
    return this.http.get<Statistic[]>(`${environment.ApiUrl}/api/statistics/new-register?key=${key}&dateFrom=${dateFrom}&dateTo=${dateTo}`,
    { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
  getRevenueDaily(key , dateFrom, dateTo) {
    return this.http.get<Revenue[]>(`${environment.ApiUrl}/api/statistics/revenue-daily?key=${key}&dateFrom=${dateFrom}&dateTo=${dateTo}`,
    { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
  getCountSalesDaily(key , dateFrom, dateTo) {
    return this.http.get<Revenue[]>(`${environment.ApiUrl}/api/statistics/count-sales-daily?key=${key}&dateFrom=${dateFrom}&dateTo=${dateTo}`,
    { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
}

