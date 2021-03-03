import { ClientRequest } from './../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from './../../../environments/environment';
import { Observable } from 'rxjs';
import { BaseService } from './base.service';
import { catchError } from 'rxjs/operators';
import jwtDecode from 'jwt-decode';
@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {
  private TokenKey = 'auth-token';
  private _sharedHeaders = new HttpHeaders();
  constructor(private http: HttpClient) {
      super();
      this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
  }
  isAuthenticated(): boolean {
    return this.getDecodedAccessToken(this.getToken()) != null;
  }
  login(client: ClientRequest): Observable<any>  {
    return this.http.post(`${environment.ApiUrl}/api/TokenAuth/Authenticate`, JSON.stringify(client),
      { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
  }
  signOut() {
    sessionStorage.clear();
  }
  saveToken(token: string) {
    sessionStorage.removeItem(this.TokenKey);
    sessionStorage.setItem(this.TokenKey, token);
  }
  getToken(): string {
    return sessionStorage.getItem(this.TokenKey);
  }
  getDecodedAccessToken(token: string): any {
    try {
        return jwtDecode(token);
    } catch (error) {
        return null;
    }
  }
}
