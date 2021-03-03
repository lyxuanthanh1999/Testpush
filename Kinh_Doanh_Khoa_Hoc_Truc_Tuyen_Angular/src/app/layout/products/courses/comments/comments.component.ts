import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../../shared';
import { Pagination, Lessons, Comments } from '../../../../shared/models';
import { LessonsService, AuthService, NotificationService } from '../../../../shared/services';
import { CommentsService } from '../../../../shared/services/comments.service';
import { CommentsDetailComponent } from '../comments-detail/comments-detail.component';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public courseId: number;
  public lessonId: number;
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
  constructor(private lessonsService: LessonsService,
    private commentService: CommentsService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private modalService: BsModalService,
    private activeRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.subscription.add(this.activeRoute.params.subscribe(params => {
      this.courseId = params['courseId'];
      this.lessonId = params['lessonId'];
    }));
    this.loadData();
  }
  checkChanged(res) {
    return res.findIndex(x => x.status === true);
  }
  loadData(selectedId = null) {
    this.blockedPanel = true;
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.role = decode.role;
    if (this.lessonId) {
      this.subscription.add(this.commentService.getAllPaging(this.lessonId, 'lessons', this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Comments>) => {
        this.processLoadData(selectedId, response);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
    } else {
    this.subscription.add(this.commentService.getAllPaging(this.courseId, 'courses', this.keyword, this.pageIndex, this.pageSize)
    .subscribe((response: Pagination<Comments>) => {
      this.processLoadData(selectedId, response);
    }, error => {
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  }
  private processLoadData(selectedId = null, response: Pagination<Comments>) {
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
    this.subscription.add(this.commentService.delete(ids).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.loadData();
      this.selectedItems = [];
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
      commentId: this.selectedItems[0].id
    };
    this.bsModalRef = this.modalService.show(CommentsDetailComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });
  }
  Back() {
    if (this.lessonId) {
      this.router.navigateByUrl('/products/courses/' + this.courseId + '/lessons');
    } else {
      this.router.navigateByUrl('/products/courses');
    }
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}

