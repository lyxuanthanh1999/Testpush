import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesDetailComponent } from './roles-detail/roles-detail.component';
import { NotificationService, RolesService } from '../../../shared/services';
import { MessageConstants } from '../../../shared';
import { Pagination, Role } from '../../../shared/models';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
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
  // Role
  public items: any[];
  public selectedItems = [];
  constructor(private rolesService: RolesService,
    private notificationService: NotificationService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(selectedId = null) {
    this.blockedPanel = true;
    this.subscription.add(this.rolesService.getAllPaging(this.keyword, this.pageIndex, this.pageSize)
      .subscribe((response: Pagination<Role>) => {
        this.processLoadData(selectedId, response);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  private processLoadData(selectedId = null, response: Pagination<Role>) {
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
    this.bsModalRef = this.modalService.show(RolesDetailComponent,
      {
        class: 'modal-lg',
        backdrop: 'static'
      });
    this.bsModalRef.content.savedEvent.subscribe((response) => {
      this.bsModalRef.hide();
      this.loadData();
      this.selectedItems = [];
    });
  }
  showEditModal() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const initialState = {
      entityId: this.selectedItems[0].id
    };
    this.bsModalRef = this.modalService.show(RolesDetailComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });

   this.subscription.add( this.bsModalRef.content.savedEvent.subscribe((response) => {
    this.bsModalRef.hide();
    this.loadData(response.id);
  }));
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
    this.subscription.add(this.rolesService.delete(ids).subscribe(() => {
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
}

