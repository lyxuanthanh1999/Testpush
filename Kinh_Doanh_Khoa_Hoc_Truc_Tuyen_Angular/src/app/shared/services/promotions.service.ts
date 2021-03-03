import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Courses, Pagination, Promotions, PromotionsRequest } from '../models';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class PromotionsService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  add(entity: PromotionsRequest) {
      return this.http.post(`${environment.ApiUrl}/api/promotions`, entity, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  update(entity: PromotionsRequest) {
      return this.http.put(`${environment.ApiUrl}/api/promotions/${entity.id}`, entity, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getDetail(id) {
      return this.http.get<Promotions>(`${environment.ApiUrl}/api/promotions/${id}`, { headers: this._sharedHeaders })
          .pipe(catchError(this.handleError));
  }

  getAllPaging(filter, pageIndex, pageSize) {
      return this.http.get<Pagination<Promotions>>(`${environment.ApiUrl}/api/promotions/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
          .pipe(map((response: Pagination<Promotions>) => {
              return response;
          }), catchError(this.handleError));
  }

  delete(ids) {
      return this.http.post(`${environment.ApiUrl}/api/promotions/delete-multi-items`, ids, { headers: this._sharedHeaders })
          .pipe(
              catchError(this.handleError)
          );
  }
  getAllPagingPromotionCourses(filter, pageIndex, pageSize) {
    return this.http.get<Pagination<Courses>>(`${environment.ApiUrl}/api/promotions/filter/courses?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
        .pipe(map((response: Pagination<Courses>) => {
            return response;
        }), catchError(this.handleError));
}
  getPromotionCourses(id: number) {
    return this.http.get<Courses[]>(`${environment.ApiUrl}/api/promotions/${id}/courses`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  removePromotionInCourses(promotionId: number, entity: number[]) {
    return this.http.post(`${environment.ApiUrl}/api/promotions/${promotionId}/courses/delete-multi-items`, entity,
    {headers: this._sharedHeaders}).pipe(catchError(this.handleError));
  }

  postPromotionInCourses(promotionId: number, entity: number[]) {
    return this.http.post(`${environment.ApiUrl}/api/promotions/${promotionId}/courses`, entity,
    { headers: this._sharedHeaders}).pipe(catchError(this.handleError));
  }
}
