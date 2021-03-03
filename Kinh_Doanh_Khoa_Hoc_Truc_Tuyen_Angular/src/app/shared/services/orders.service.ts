import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Pagination } from '../models';
import { Order } from '../models/oder.model';
import { BaseService } from './base.service';
import { OrderDetail } from '../models/order-detail.model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService extends BaseService {
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  getDetail(id) {
    return this.http.get<Order>(`${environment.ApiUrl}/api/orders/${id}`,
     { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}

export(entity) {
  return this.http.post(`${environment.ApiUrl}/api/orders/export-excel`, entity,
   { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}

getAllPaging(filter, pageIndex, pageSize) {
    return this.http.get<Pagination<Order>>(`${environment.ApiUrl}/api/orders/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`,
     { headers: this._sharedHeaders })
     .pipe(map((response: Pagination<Order>) => {
       return response;
    }), catchError(this.handleError));
}

updateStatusOrder(type, ids) {
  return this.http.put(`${environment.ApiUrl}/api/orders/status-type-${type}/multi-items`, ids,
   { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}

getOrderDetail(orderId, activeCourseId) {
  return this.http.get<OrderDetail>(`${environment.ApiUrl}/api/orders/${orderId}/order-details/${activeCourseId}`,
   { headers: this._sharedHeaders })
      .pipe(catchError(this.handleError));
}

getOrderDetailAllPaging(orderId, filter, pageIndex, pageSize) {
  return this.http.get<Pagination<OrderDetail>>(`${environment.ApiUrl}/api/orders/${orderId}/order-details/filter?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${filter}`, { headers: this._sharedHeaders })
      .pipe(map((response: Pagination<OrderDetail>) => {
          return response;
      }), catchError(this.handleError));
}

deleteOrderDetail(orderId, ids) {
    return this.http.post(`${environment.ApiUrl}/api/orders/${orderId}/order-details/delete-multi-items`, ids,
     { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}
updateOrderDetail(entity) {
  return this.http.put(`${environment.ApiUrl}/api/orders/${entity.orderId}/order-details/${entity.activeCourseId}`, entity,
   { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
}


}
