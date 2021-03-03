import { Courses } from './../models/course.model';
import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { CoursesRequest } from '../models/course-request.model';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Pagination } from '../models';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class CoursesService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(entity) {
      return this.http.post(`${environment.ApiUrl}/api/courses`, entity,
      {
        reportProgress: true,
        observe: 'events'
      })
      .pipe(catchError(this.handleError));
  }

  update(id: number, entity) {
      return this.http.put(`${environment.ApiUrl}/api/courses/${id}`, entity,
      {
        reportProgress: true,
        observe: 'events'
      })
      .pipe(catchError(this.handleError));
  }

  approve(entity: any) {
    return this.http.put(`${environment.ApiUrl}/api/courses/approve`, entity, { headers: this._sharedHeaders })
        .pipe(catchError(this.handleError));
}

  getDetail(id) {
      return this.http.get<Courses>(`${environment.ApiUrl}/api/courses/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getAllPaging(filter, pageIndex, pageSize) {
      return this.http.get<Pagination<Courses>>(`${environment.ApiUrl}/api/courses/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
          .pipe(map((response: Pagination<Courses>) => {
              return response;
          }), catchError(this.handleError));
  }

  delete(ids) {
      return this.http.post(`${environment.ApiUrl}/api/courses/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }
  deleteAttachment(id) {
    return this.http.delete(`${environment.ApiUrl}/api/courses/${id}/image`, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }
  getAll() {
      return this.http.get<Courses[]>(`${environment.ApiUrl}/api/courses`, { headers: this._sharedHeaders})
      .pipe(map((response: Courses[]) => {
          return response;
      }), catchError(this.handleError));
  }

  getActiveCourses(id: number) {
    return this.http.get<User[]>(`${environment.ApiUrl}/api/courses/${id}/users`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  removeActiveCourses(coursesId: number, entity: string[]) {
    return this.http.post(`${environment.ApiUrl}/api/courses/${coursesId}/users/delete-multi-items`, entity,
    {headers: this._sharedHeaders}).pipe(catchError(this.handleError));
  }

  postActiveCourses(coursesId: number, entity: string[]) {
    return this.http.post(`${environment.ApiUrl}/api/courses/${coursesId}/users`, entity,
    { headers: this._sharedHeaders}).pipe(catchError(this.handleError));
  }
  activeCourses(coursesId: number, entity: string[]) {
    return this.http.put(`${environment.ApiUrl}/api/courses/${coursesId}/users/active`, entity,
    { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}

}
