import { Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../../shared';
import { environment } from '../../../../../environments/environment';
import { Pagination, Courses } from '../../../../shared/models';
import { NotificationService, PromotionsService } from '../../../../shared/services';

@Component({
  selector: 'app-courses-in-promotion',
  templateUrl: './courses-in-promotion.component.html',
  styleUrls: ['./courses-in-promotion.component.scss']
})
export class CoursesInPromotionComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public checkApprove = false;
  // Default
  public blockedPanel = false;
  public title: string;
  private chosenEvent: EventEmitter<any> = new EventEmitter();
  /**
   * Paging
   */
  public pageIndex = 1;
  public pageSize = 5;
  public totalRecords: number;
  public keyword = '';
  public items: any[];
  public selectedItems = [];
  public promotionId: number;
  public backendApiUrl = environment.ApiUrl;
  public existingCourses: any[];
  constructor(public bsModalRef: BsModalRef,
    private notificationService: NotificationService,
    private promotionsService: PromotionsService) { }

  ngOnInit(): void {
    this.loadData();
  }
  checkChanged(res) {
    return res.findIndex(x => x.status === 1);
  }
  loadData() {
    this.blockedPanel = true;
    this.subscription.add(this.promotionsService.getAllPagingPromotionCourses(this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Courses>) => {
        this.processLoadData(response);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(response: Pagination<Courses>) {
    const existingCourses = this.existingCourses;
    this.pageIndex = this.pageIndex;
    this.pageSize = this.pageSize;
    this.totalRecords = response.totalRecords;
    const newCourses = response.items.filter(function (item) {
      return existingCourses.indexOf(item.id) === -1;
    });
    this.items = newCourses;
    if (this.selectedItems.length === 0 && this.items.length > 0) {
      this.selectedItems.push(this.items[0]);
    }
    setTimeout(() => { this.blockedPanel = false; }, 1000);
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  chooseCourses() {
    this.blockedPanel = true;
    const selectedIds = [];
    this.selectedItems.forEach(element => {
      selectedIds.push(element.id);
    });
    this.subscription.add(this.promotionsService.postPromotionInCourses(this.promotionId, selectedIds).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Created_Ok);
      this.chosenEvent.emit(this.selectedItems);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
}
