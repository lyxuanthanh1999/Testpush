import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import * as signalR from '@microsoft/signalr';
import { environment } from './../../../environments/environment';
import { Announcement } from '../models';
import { Subject, Observable } from 'rxjs';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService extends BaseService {

  private receivedMessageObject: Announcement = new Announcement();
  private sharedObj = new Subject<Announcement>();
  private token = this.authService.getToken();
  private options: IHttpConnectionOptions = {
    accessTokenFactory: () => {
      return this.token;
    },
    transport: signalR.HttpTransportType.LongPolling
  };

  private connection: any = new signalR.HubConnectionBuilder().withUrl(`${environment.ApiUrl}/notification`, this.options)
  .configureLogging(signalR.LogLevel.Information)
    .build();

  constructor(private authService: AuthService) {
    super();
    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on('ReceiveMessage', (message: Announcement) => { this.mapReceivedMessage(message); });
    this.connection.on('ReceiveMessageFromServer', (lstUserId: string[], message: Announcement) => {
       this.mapReceivedMessageFromServer(lstUserId, message); });

    this.start();
  }

  public async start() {
    try {
      await this.connection.start();
      console.log('connected');
    } catch (err) {
      console.log(err);
    //  setTimeout(() => this.start(), 5000);
    }
  }

  private mapReceivedMessage(message: Announcement): void {
    this.receivedMessageObject = message;
    this.sharedObj.next(this.receivedMessageObject);
  }

  private mapReceivedMessageFromServer(lstUserId: string[], message: Announcement): void {
    const decode = this.authService.getDecodedAccessToken(this.token);
    const userId = decode.sub;
    if (lstUserId.indexOf(userId) !== -1) {
      this.receivedMessageObject = message;
      this.sharedObj.next(this.receivedMessageObject);
    }
  }

  public disconnect() {
    this.connection.stop();
  }

  /* ****************************** Public Mehods **************************************** */

  public SendMessage(method: string, message: Announcement) {
    this.connection.invoke(method, message).catch(err => console.error(err));
  }

  public SendMessageToUser(method: string, userId: string, message: Announcement) {
    this.connection.invoke(method, userId, message).catch(err => console.error(err));
  }

  public retrieveMappedObject(): Observable<Announcement> {
    return this.sharedObj.asObservable();
  }

}
