import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { Pagination, Announcement, AnnouncementMarkReadRequest } from '../../../shared/models';
import { AnnouncementService, AuthService, NotificationService } from '../../../shared/services';
import { AnnouncementDetailComponent } from './announcement-detail/announcement-detail.component';


@Component({
  selector: 'app-announcement',
  templateUrl: './announcement.component.html',
  styleUrls: ['./announcement.component.scss']
})
export class AnnouncementComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  // Default
  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  /**
   * Paging
   */
  public userId: string;
  public pageIndex = 1;
  public pageSize = 5;
  public totalRecords: number;
  public keyword = 'true';
  public items: any[];
  public chooseFilter =
  [
    {value: 'true', label: 'Tất cả'},
    {value: 'false', label: 'Thông báo chưa đọc'},
  ];
  public selectedItems: any;
  constructor(private announcementService: AnnouncementService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData(selectedId = null) {
    this.blockedPanel = true;
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.userId = decode.sub;
    this.subscription.add(this.announcementService.getAllPaging(this.keyword, this.userId, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Announcement>) => {
        this.processLoadData(selectedId, response);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  loadFilter() {
    this.loadData();
  }
  private processLoadData(selectedId = null, response: Pagination<Announcement>) {
    this.items = response.items;
    this.pageIndex = this.pageIndex;
    this.pageSize = this.pageSize;
    this.totalRecords = response.totalRecords;
    this.selectedItems = this.items[0];
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
  markAsRead() {
    this.blockedPanel = true;
    const entity = new AnnouncementMarkReadRequest();
    entity.announceId = this.selectedItems.id;
    entity.userId = this.userId;
    this.subscription.add(this.announcementService.updateMaskAsRead(entity).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Updated_Ok);
      this.loadData();
      this.selectedItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  showDetailModal() {
    if (!this.selectedItems) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const initialState = {
      announceId: this.selectedItems.id
    };
    this.bsModalRef = this.modalService.show(AnnouncementDetailComponent,
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

