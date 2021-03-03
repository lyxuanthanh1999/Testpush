import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../../shared';
import { Lessons, Pagination } from '../../../../shared/models';
import { AuthService, LessonsService, NotificationService, SignalRService } from '../../../../shared/services';

@Component({
  selector: 'app-lessons',
  templateUrl: './lessons.component.html',
  styleUrls: ['./lessons.component.scss']
})
export class LessonsComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public checkApprove = false;
  public entityId: number;
  // Default
  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  /**
   * Paging
   */
  public pageIndex = 1;
  public pageSize = 100;
  public totalRecords: number;
  public keyword = '';
  public items: any[];
  public selectedItems = [];
  public role: string;
  constructor(private lessonsService: LessonsService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private modalService: BsModalService,
    private signalRSevice: SignalRService,
    private activeRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.subscription.add(this.activeRoute.params.subscribe(params => {
      this.entityId = params['coursesId'];
    }));
    this.loadData();
  }
  checkChanged(res) {
    return res.findIndex(x => x.status === 1);
  }
  loadData(selectedId = null) {
    this.blockedPanel = true;
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.role = decode.role;
    this.subscription.add(this.lessonsService.getAllPaging(this.entityId, this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Lessons>) => {
        this.processLoadData(selectedId, response);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(selectedId = null, response: Pagination<Lessons>) {
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
  showAddModal() {
    this.router.navigateByUrl('/products/courses/' + this.entityId + '/lessons/lessons-detail');
  }
  showEditModal() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    this.router.navigateByUrl('/products/courses/' + this.entityId + '/lessons/lessons-detail/' + this.selectedItems[0].id);
  }

  deleteItems() {
    const ids = [];
    this.selectedItems.map(data => {
      ids.push(data.id);
    });
    this.notificationService.showConfirmation(MessageConstants.Confirm_Delete,
      () => this.deleteItemsConfirm(ids));
  }
  deleteItemsConfirm(ids: any[]) {
    this.blockedPanel = true;
    this.subscription.add(this.lessonsService.delete(ids).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.loadData();
      this.selectedItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  Back() {
    this.router.navigateByUrl('/products/courses');
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  approve() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const entity = [];
    this.selectedItems.map(data => {
      entity.push(data.id);
    });
    this.subscription.add(this.lessonsService.approve(entity).subscribe((response: any[]) => {
      if (response.length > 0) {
        response.map(data => {
          this.signalRSevice.SendMessageToUser('SendToUserAsync', data.userId, data.announcementViewModel);
        });
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess(MessageConstants.Approve_Ok);
      }
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  viewComment() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    this.router.navigateByUrl('/products/courses/' + this.entityId + '/lessons/' + this.selectedItems[0].id + '/comments');
  }
}

