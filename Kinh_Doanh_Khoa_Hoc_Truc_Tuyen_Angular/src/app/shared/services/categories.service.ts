import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Category } from '../models';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(entity: Category) {
      return this.http.post(`${environment.ApiUrl}/api/categories`, JSON.stringify(entity), { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  update(id: string, entity: Category) {
      return this.http.put(`${environment.ApiUrl}/api/categories/${id}`, JSON.stringify(entity), { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getDetail(id) {
      return this.http.get<Category>(`${environment.ApiUrl}/api/categories/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  delete(ids: any[]) {
      return this.http.post(`${environment.ApiUrl}/api/categories/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }

  getAll() {
      return this.http.get<Category[]>(`${environment.ApiUrl}/api/categories`, { headers: this._sharedHeaders })
          .pipe(map((response: Category[]) => {
              return response;
          }), catchError(this.handleError));
  }


  getAllByParentId(parentId) {
      let url = '';
      if (parentId) {
          url = `${environment.ApiUrl}/api/categories/${parentId}/parents`;
      } else {
          url = `${environment.ApiUrl}/api/categories/`;
      }

      return this.http.get<Category[]>(url, { headers: this._sharedHeaders })
          .pipe(map((response: Category[]) => {
              return response;
          }), catchError(this.handleError));
  }
}
