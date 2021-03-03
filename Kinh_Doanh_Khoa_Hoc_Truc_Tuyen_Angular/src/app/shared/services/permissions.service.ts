import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from './../../../environments/environment';
import { PermissionUpdateRequest, PermissionScreen } from '../models';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionsService  extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  save(roleId: string, request: PermissionUpdateRequest) {
      return this.http.put(`${environment.ApiUrl}/api/roles/${roleId}/permissions`, JSON.stringify(request),
          { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getFunctionWithCommands() {
      return this.http.get<PermissionScreen>(`${environment.ApiUrl}/api/permissions`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }
}
