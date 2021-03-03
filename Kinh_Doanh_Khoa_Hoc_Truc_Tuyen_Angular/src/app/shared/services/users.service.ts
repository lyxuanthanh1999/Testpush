import { Pagination } from './../models/pagination.model';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { User, Function, UserChangePassword } from '../models';
import { UtilitiesService } from './utilities.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient, private utilitiesService: UtilitiesService) {
    super();
    this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');

}

  resetPassword(id: string) {
    return this.http.get(`${environment.ApiUrl}/api/users/${id}/administrator/reset-password`, { headers: this._sharedHeaders })
    .pipe(catchError(this.handleError));
  }

  updatePassword(entity: UserChangePassword) {
    return this.http.put(`${environment.ApiUrl}/api/users/${entity.id}/change-password`, entity, { headers: this._sharedHeaders })
    .pipe(catchError(this.handleError));
  }

  deleteAvatar(id: string) {
    return this.http.delete(`${environment.ApiUrl}/api/users/${id}-delete-avatar`,
     { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }

  update(id: string, entity) {
    return this.http.put(`${environment.ApiUrl}/api/users/${id}`, entity,   {
      reportProgress: true,
      observe: 'events'
    })
    .pipe(catchError(this.handleError));
  }

  getDetail(id) {
    return this.http.get<User>(`${environment.ApiUrl}/api/users/${id}`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  add(entity) {
    return this.http.post(`${environment.ApiUrl}/api/users/server`, entity,   {
      reportProgress: true,
      observe: 'events'
    })
    .pipe(catchError(this.handleError));
  }

  getAllPaging(filter, pageIndex, pageSize) {
    return this.http.get<Pagination<User>>(`${environment.ApiUrl}/api/users/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, {headers: this._sharedHeaders})
    .pipe(map((response: Pagination<User>) => {
      return response;
    }), catchError(this.handleError));
  }

  delete(ids: any[]) {
    return this.http.post(`${environment.ApiUrl}/api/users/delete-multi-items`, ids, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  getMenuByUser(userId: string) {
    return this.http.get<Function[]>(`${environment.ApiUrl}/api/users/${userId}/menu`, {headers: this._sharedHeaders})
    .pipe(map(response => {
      const functions = this.utilitiesService.UnflatteringForLeftMenu(response);
      return functions;
    }), catchError(this.handleError));
  }

  getUserRoles(userId: string) {
    return this.http.get<string[]>(`${environment.ApiUrl}/api/users/${userId}/roles`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  removeRolesFromUser(id, roleNames: string[]) {
    let rolesQuery = '';
    for (const roleName of roleNames) {
      rolesQuery += 'roleNames' + '=' + roleName + '&';
    }
    return this.http.delete(`${environment.ApiUrl}/api/users/${id}/roles?${rolesQuery}`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  assignRolesToUser(userId: string, assignRolesToUser: any) {
    return this.http.post(`${environment.ApiUrl}/api/users/${userId}/roles`
    , JSON.stringify(assignRolesToUser), { headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }
}
