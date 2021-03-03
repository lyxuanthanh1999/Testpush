import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { Pagination, Comments } from '../../../shared/models';
import { AuthService, NotificationService, SignalRService } from '../../../shared/services';
import { CommentsService } from '../../../shared/services/comments.service';
import { OrdersService } from '../../../shared/services/orders.service';
import { Order } from '../../../shared/models/oder.model';
import { CommentsDetailComponent } from '../courses/comments-detail/comments-detail.component';
import { OrdersDetailComponent } from './orders-detail/orders-detail.component';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  // public courseId: number;
  // public lessonId: number;
  // Default
  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  /**
   * Paging
   */
  public pageIndex = 1;
  public pageSize = 5;
  public totalRecords: number;
  public keyword = '';
  public items: any[];
  public selectedItems = [];
  public role: string;
  constructor(private orderService: OrdersService,
    private commentService: CommentsService,
    private authService: AuthService,
    private signalRSevice: SignalRService,
    private notificationService: NotificationService,
    private modalService: BsModalService,
    private activeRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.loadData();
  }
  checkChanged(res, status) {
    return res.findIndex(x => x.status === status);
  }
  loadData(selectedId = null) {
    this.blockedPanel = true;
    this.subscription.add(this.orderService.getAllPaging(this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Order>) => {
        this.processLoadData(selectedId, response);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(selectedId = null, response: Pagination<Order>) {
    this.items = response.items;
    this.pageIndex = this.pageIndex;
    this.pageSize = this.pageSize;
    this.totalRecords = response.totalRecords;
    if (this.selectedItems.length === 0 && this.items.length > 0) {
      this.selectedItems.push(this.items[0]);
    }
    if (selectedId != null && this.items.length > 0) {
      this.selectedItems = this.items.filter(x => x.Id === selectedId);
    }
    setTimeout(() => { this.blockedPanel = false; }, 1000);
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }
  confirmStatusItems(status: number) {
    const ids = [];
    this.selectedItems.map(data => {
      ids.push(data.id);
    });
    if (status === 3) {
      this.notificationService.showConfirmation(MessageConstants.Confirm_Update,
        () => this.confirmStatusConfirm(status, ids));
    } else if (status === 2) {
      this.notificationService.showConfirmation(MessageConstants.Confirm_Update,
        () => this.confirmStatusConfirm(status, ids));
    } else if (status === 1) {
      this.notificationService.showConfirmation(MessageConstants.Confirm_Update,
        () => this.confirmStatusConfirm(status, ids));
    } else {
      this.notificationService.showConfirmation(MessageConstants.Confirm_Update,
        () => this.confirmStatusConfirm(status, ids));
    }
  }
  confirmStatusConfirm(status: number, ids: any[]) {
    this.blockedPanel = true;
    this.subscription.add(this.orderService.updateStatusOrder(status, ids).subscribe((response: any[]) => {
      console.log(response);
      if (response.length > 0) {
        response.map(data => {
          this.signalRSevice.SendMessageToUser('SendToUserAsync', data.userId, data.announcementViewModel);
        });
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess(MessageConstants.Updated_Ok);
      }
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  showDetailModal() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const initialState = {
      orderId: this.selectedItems[0].id
    };
    this.bsModalRef = this.modalService.show(OrdersDetailComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}

