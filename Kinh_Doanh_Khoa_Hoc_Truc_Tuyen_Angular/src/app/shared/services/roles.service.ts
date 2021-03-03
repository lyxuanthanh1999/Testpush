import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { catchError, map } from 'rxjs/operators';
import { Pagination, Permission, Role } from '../models';
import { stringify } from 'querystring';

@Injectable({
  providedIn: 'root'
})
export class RolesService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(name: string) {
      return this.http.post(`${environment.ApiUrl}/api/roles?name=${name}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  update(id: string, name: string) {
      const body = {id: id, name: name};
      return this.http.put(`${environment.ApiUrl}/api/roles/${id}`, body, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getDetail(id) {
      return this.http.get<Role>(`${environment.ApiUrl}/api/roles/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getAllPaging(filter, pageIndex, pageSize) {
      return this.http.get<Pagination<Role>>(`${environment.ApiUrl}/api/roles/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
          .pipe(map((response: Pagination<Role>) => {
              return response;
          }), catchError(this.handleError));
  }

  delete(ids) {
      return this.http.post(`${environment.ApiUrl}/api/roles/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }

  getAll() {
      return this.http.get<Role[]>(`${environment.ApiUrl}/api/roles`, { headers: this._sharedHeaders})
      .pipe(map((response: Role[]) => {
          return response;
      }), catchError(this.handleError));
  }

  getRolePermissions(roleId) {
    return this.http.get<Permission[]>(`${environment.ApiUrl}/api/roles/${roleId}/permissions`, { headers: this._sharedHeaders })
        .pipe(catchError(this.handleError));
}
}
