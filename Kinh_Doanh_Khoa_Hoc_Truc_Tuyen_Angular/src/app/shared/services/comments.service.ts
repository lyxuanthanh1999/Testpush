import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Comments, Lessons, Pagination } from '../models';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class CommentsService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  getDetail(commentId) {
    return this.http.get<Comments>(`${environment.ApiUrl}/api/comments/${commentId}`,
     { headers: this._sharedHeaders })
        .pipe(catchError(this.handleError));
}

getAllPaging(entityId, entityType, filter, pageIndex, pageSize) {
    return this.http.get<Pagination<Comments>>(`${environment.ApiUrl}/api/comments/${entityType}/${entityId}/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
        .pipe(map((response: Pagination<Comments>) => {
            return response;
        }), catchError(this.handleError));
}

delete(ids) {
    return this.http.post(`${environment.ApiUrl}/api/comments/delete-multi-items`, ids, { headers: this._sharedHeaders })
        .pipe(
            catchError(this.handleError)
        );
}
}
