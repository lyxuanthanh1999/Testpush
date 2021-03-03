import { EventEmitter } from '@angular/core';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { Pagination, User } from '../../../../shared/models';
import { CoursesService } from '../../../../shared/services';
import { UsersService } from '../../../../shared/services/users.service';

@Component({
  selector: 'app-active-courses',
  templateUrl: './active-courses.component.html',
  styleUrls: ['./active-courses.component.scss']
})
export class ActiveCoursesComponent implements OnInit, OnDestroy {
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
  public userId: number;
  public existingUsers: any[];
  constructor(public bsModalRef: BsModalRef,
    private usersService: UsersService,
    private coursesService: CoursesService) { }

  ngOnInit(): void {
    this.loadData();
  }
  checkChanged(res) {
    return res.findIndex(x => x.status === 1);
  }
  loadData() {
    this.blockedPanel = true;
    this.subscription.add(this.usersService.getAllPaging(this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<User>) => {
        this.processLoadData(response);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(response: Pagination<User>) {
    const existingUsers = this.existingUsers;
    this.pageIndex = this.pageIndex;
    this.pageSize = this.pageSize;
    this.totalRecords = response.totalRecords;
    const newUsers = response.items.filter(function (item) {
      return existingUsers.indexOf(item.id) === -1;
    });
    this.items = newUsers;
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
    this.coursesService.postActiveCourses(this.userId, selectedIds).subscribe(() => {
      this.chosenEvent.emit(this.selectedItems);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    });
  }
}
