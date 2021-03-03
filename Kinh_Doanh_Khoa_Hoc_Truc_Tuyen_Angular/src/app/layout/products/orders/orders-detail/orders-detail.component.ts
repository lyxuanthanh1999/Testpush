import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { OrdersService } from '../../../../shared/services/orders.service';
import { Order } from '../../../../shared/models/oder.model';
import { MessageConstants } from '../../../../shared';
import { NotificationService } from '../../../../shared/services';
import * as fileSaver from 'file-saver';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-orders-detail',
  templateUrl: './orders-detail.component.html',
  styleUrls: ['./orders-detail.component.scss']
})
export class OrdersDetailComponent implements OnInit, OnDestroy {
  constructor(
    public bsModalRef: BsModalRef,
    private notificationService: NotificationService,
    private ordersService: OrdersService) {
  }
  public backendApiUrl = environment.ApiUrl;
  public totalRecords: number;
  private subscription = new Subscription();
  public orderId: number;
  public btnDisabled = false;
  public blockedPanel = false;
  public res: Order;
  public selectedItems = [];
  ngOnInit() {
    if (this.orderId) {
      this.loadFormDetails(this.orderId);
    }
  }
  private loadFormDetails(orderId) {
    this.blockedPanel = true;
    this.subscription.add(this.ordersService.getDetail(orderId)
      .subscribe((response: Order) => {
        this.res = response;
        this.totalRecords = response.orderDetails.length;
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }));
  }
  export() {
    this.blockedPanel = true;
    this.subscription.add(this.ordersService.export(this.res)
      .subscribe((response: any) => {
        const urlPdf = this.backendApiUrl + '/attachments/export-files/' + response.filePdf;
        window.open(urlPdf);
        const urlExcel = this.backendApiUrl + '/attachments/export-files/' + response.fileExcel;
        setTimeout(() => { window.open(urlExcel); }, 1000);
        this.notificationService.showSuccess(MessageConstants.Export_File_Ok);
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 2000);

      }, error => {
        this.notificationService.showError(MessageConstants.Export_File_Failed);
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }));
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
