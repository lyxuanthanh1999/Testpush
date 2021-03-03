import { Courses } from './../../../shared/models/course.model';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { CoursesService } from '../../../shared/services/courses.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { AnnouncementCreateRequest, Pagination } from '../../../shared/models';
import { AuthService, NotificationService, SignalRService } from '../../../shared/services';
import { CoursesDetailComponent } from './courses-detail/courses-detail.component';
import { Router } from '@angular/router';
import { ActiveCoursesComponent } from './active-courses/active-courses.component';
import { environment } from '../../../../environments/environment';
@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  public checkApprove = false;
  // Default
  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  public blockedPanelActiveCourses = false;
  public showActivateCourses = false;
  /**
   * Paging
   */
  public role: string;
  public pageIndex = 1;
  public pageSize = 5;
  public totalRecords: number;
  public keyword = '';
  public items: any[];
  public selectedItems = [];
  public selectedUserItems = [];
  public users: any[] = [];
  public totalUsersRecords: number;
  public backendApiUrl = environment.ApiUrl;
  constructor(private coursesService: CoursesService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private modalService: BsModalService,
    private signalRSevice: SignalRService,
    private router: Router) { }

  ngOnInit(): void {
    this.loadData();
  }
  checkChanged(res) {
    return res.findIndex(x => x.status === 1);
  }
  loadData(selectedId = null) {
    this.blockedPanel = true;
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.role = decode.role;
    this.subscription.add(this.coursesService.getAllPaging(this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Courses>) => {
        this.processLoadData(selectedId, response);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(selectedId = null, response: Pagination<Courses>) {
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
  showHideActivateCoursesTable() {
    if (this.showActivateCourses) {
      if (this.selectedItems.length === 1) {
        this.loadActivateCourses();
      }
    }
  }
  loadActivateCourses() {
    this.blockedPanelActiveCourses = true;
    if (this.selectedItems != null && this.selectedItems.length > 0) {
      const courseId = this.selectedItems[0].id;
      this.coursesService.getActiveCourses(courseId).subscribe((response: any) => {
        this.users = response;
        this.totalUsersRecords = response.length;
        if (this.selectedUserItems.length === 0 && this.users.length > 0) {
          this.selectedUserItems.push(this.users[0]);
        }
        setTimeout(() => { this.blockedPanelActiveCourses = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanelActiveCourses = false; }, 1000);
      });
    } else {
      this.selectedUserItems = [];
      setTimeout(() => { this.blockedPanelActiveCourses = false; }, 1000);
    }
  }

  onRowSelect(event) {
    this.selectedUserItems = [];
    this.totalUsersRecords = 0;
    this.users = [];
    if (this.selectedItems.length === 1 && this.showActivateCourses) {
      this.loadActivateCourses();
    }
  }

  onRowUnselect(event) {
    this.selectedUserItems = [];
    this.totalUsersRecords = 0;
    this.users = [];
    if (this.selectedItems.length === 1 && this.showActivateCourses) {
      this.loadActivateCourses();
    }
  }

  addActiveCourse() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const existingUsersId = [];
    this.users.map(res => {
      existingUsersId.push(res.id);
    });
    const initialState = {
      existingUsers: existingUsersId,
      userId: this.selectedItems[0].id
    };
    this.bsModalRef = this.modalService.show(ActiveCoursesComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });
    this.bsModalRef.content.chosenEvent.subscribe((response: any[]) => {
      this.bsModalRef.hide();
      this.loadActivateCourses();
      this.selectedUserItems = [];
    });
  }

  removeActiveCourse() {
    this.notificationService.showConfirmation(MessageConstants.Confirm_Delete,
      () => this.deleteActiveCourseConfirm(this.selectedUserItems));
  }

  deleteActiveCourseConfirm(selectedUsers) {
    this.blockedPanelActiveCourses = true;
    const Ids = [];
    selectedUsers.map(res => {
      Ids.push(res.id);
    });
    this.coursesService.removeActiveCourses(this.selectedItems[0].id, Ids).subscribe(() => {
      this.loadActivateCourses();
      this.selectedUserItems = [];
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      setTimeout(() => { this.blockedPanelActiveCourses = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanelActiveCourses = false; }, 1000);
    });
  }

  activeCourses() {
    if (this.selectedUserItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const entity = [];
    this.selectedUserItems.map(data => {
      entity.push(data.id);
    });
    this.subscription.add(this.coursesService.activeCourses(this.selectedItems[0].id, entity).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Updated_Ok);
      this.loadActivateCourses();
      this.selectedUserItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }

  showAddModal() {
    this.router.navigateByUrl('/products/courses/courses-detail');
  }
  showEditModal() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    this.router.navigateByUrl('/products/courses/courses-detail/' + this.selectedItems[0].id);
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
    this.subscription.add(this.coursesService.delete(ids).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.loadData();
      this.selectedItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(error);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
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
    this.subscription.add(this.coursesService.approve(entity).subscribe((response: any[]) => {
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
  viewLesson() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    this.router.navigateByUrl('/products/courses/' + this.selectedItems[0].id + '/lessons');
  }
  viewComment() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    this.router.navigateByUrl('/products/courses/' + this.selectedItems[0].id + '/comments');
  }
}

