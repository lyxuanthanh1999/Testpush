import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from './../../../environments/environment';
import { CommandAssign } from '../models';
import { BaseService } from './base.service';


@Injectable({
  providedIn: 'root'
})
export class FunctionsService extends BaseService {

  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(entity: Function) {
      return this.http.post(`${environment.ApiUrl}/api/functions`, JSON.stringify(entity), { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  update(id: string, entity: Function) {
      return this.http.put(`${environment.ApiUrl}/api/functions/${id}`, JSON.stringify(entity), { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getDetail(id) {
      return this.http.get<Function>(`${environment.ApiUrl}/api/functions/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  delete(ids: any[]) {
      return this.http.post(`${environment.ApiUrl}/api/functions/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }

  getAll() {
      return this.http.get<Function[]>(`${environment.ApiUrl}/api/functions`, { headers: this._sharedHeaders })
          .pipe(map((response: Function[]) => {
              return response;
          }), catchError(this.handleError));
  }


  getAllByParentId(parentId) {
      let url = '';
      if (parentId) {
          url = `${environment.ApiUrl}/api/functions/${parentId}/parents`;
      } else {
          url = `${environment.ApiUrl}/api/functions/`;
      }

      return this.http.get<Function[]>(url, { headers: this._sharedHeaders })
          .pipe(map((response: Function[]) => {
              return response;
          }), catchError(this.handleError));
  }

  getAllCommandsByFunctionId(functionId: string) {
      return this.http.get<Function[]>(`${environment.ApiUrl}/api/functions/${functionId}/commands`, { headers: this._sharedHeaders })
          .pipe(map((response: Function[]) => {
              return response;
          }), catchError(this.handleError));
  }
  addCommandsToFunction(functionId, commandAssign: CommandAssign) {
      return this.http.post(`${environment.ApiUrl}/api/functions/${functionId}/commands/`, JSON.stringify(commandAssign)
          , { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }
  deleteCommandsFromFunction(functionId, commandAssign: CommandAssign) {
      return this.http.post(`${environment.ApiUrl}/api/functions/${functionId}/commands/delete-items`, commandAssign,
      { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
}
