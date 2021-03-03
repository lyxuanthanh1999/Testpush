import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { AnnouncementMarkReadRequest, Announcement, Pagination, AnnouncementCreateRequest } from '../models';
import { BaseService } from './base.service';


@Injectable({
  providedIn: 'root'
})
export class AnnouncementService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
    super();
    this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }

  postAnnouncement(entity: AnnouncementCreateRequest) {
    return this.http.post(`${environment.ApiUrl}/api/announcements/create-announce`
    , JSON.stringify(entity), {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  getAnnounceForServer(id: string) {
    return this.http.get(`${environment.ApiUrl}/api/announcements/send-announce-${id}/server`, {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  updateMaskAsRead(entity: AnnouncementMarkReadRequest) {
    return this.http.put(`${environment.ApiUrl}/api/announcements/mark-read`, JSON.stringify(entity), {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  getDetail(id, userId) {
    return this.http.get<Announcement>(`${environment.ApiUrl}/api/announcements/${id}?receiveId=${userId}&announceId=${id}`,
    {headers: this._sharedHeaders})
    .pipe(catchError(this.handleError));
  }

  getAllPaging(filter, userId, pageIndex, pageSize) {
    return this.http.get<Pagination<Announcement>>(`${environment.ApiUrl}/api/announcements/private-paging/filter?userId=${userId}&pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, {headers: this._sharedHeaders})
    .pipe(map((response: Pagination<Announcement>) => {
      return response;
    }), catchError(this.handleError));
  }
}
