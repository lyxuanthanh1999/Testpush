import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Pagination } from '../models';
import { LessonsRequest } from '../models/lessons-request.model';
import { Lessons } from '../models/lessons.model';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class LessonsService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(entity) {
      return this.http.post(`${environment.ApiUrl}/api/lessons`, entity,
      {
        reportProgress: true,
        observe: 'events'
      })
    .pipe(catchError(this.handleError));
  }

  update(id: number, entity) {
      return this.http.put(`${environment.ApiUrl}/api/lessons/${id}`, entity,
      {
        reportProgress: true,
        observe: 'events'
      })
    .pipe(catchError(this.handleError));
  }

  approve(entity: any) {
    return this.http.put(`${environment.ApiUrl}/api/lessons/approve`, entity, { headers: this._sharedHeaders })
        .pipe(catchError(this.handleError));
}

  getDetail(id) {
      return this.http.get<Lessons>(`${environment.ApiUrl}/api/lessons/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getAllPaging(id, filter, pageIndex, pageSize) {
      return this.http.get<Pagination<Lessons>>(`${environment.ApiUrl}/api/lessons/filter?courseId=${id}&pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
          .pipe(map((response: Pagination<Lessons>) => {
              return response;
          }), catchError(this.handleError));
  }

  delete(ids) {
      return this.http.post(`${environment.ApiUrl}/api/lessons/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }
  deleteAttachment(coursesId: number, lessonId: number) {
    return this.http.delete(`${environment.ApiUrl}/api/lessons/course-${coursesId}-lesson-${lessonId}/attachment`,
     { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
  deleteVideo(coursesId: number, lessonId: number) {
    return this.http.delete(`${environment.ApiUrl}/api/lessons/course-${coursesId}-lesson-${lessonId}/video`,
    { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
}
