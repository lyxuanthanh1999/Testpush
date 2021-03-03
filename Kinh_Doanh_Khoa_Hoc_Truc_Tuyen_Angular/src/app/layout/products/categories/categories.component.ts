
import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { CategoriesService, NotificationService, UtilitiesService } from '../../../shared/services';
import { CategoriesDetailComponent } from './categories-detail/categories-detail.component';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  public blockedPanelCommand = false;
  public showCommandGrid = false;
  // -----------------Categories-----------------
  public items: TreeNode[] = [];
  public selectedItems = [];
  public prevSelectItems = [];
  constructor(
    private modalService: BsModalService,
    private categoriesService: CategoriesService,
    private notificationService: NotificationService,
    private utilitiesService: UtilitiesService) {
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();

  }

  ngOnInit() {
    this.loadData();
  }

  loadData(selectionId = null) {
    this.blockedPanel = true;
    this.subscription = this.categoriesService.getAll()
      .subscribe((response: any) => {
        const functionTree = this.utilitiesService.UnflatteringForTree(response);
        this.items = <TreeNode[]>functionTree;
        if (this.selectedItems.length === 0 && this.items.length > 0) {
          this.selectedItems.push(this.items[0]);
        }
        // Nếu có là sửa thì chọn selection theo Id
        if (selectionId != null && this.items.length > 0) {
          this.selectedItems = this.items.filter(x => x.data.id === selectionId);
        }
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      });
  }

  showAddModal() {
    this.bsModalRef = this.modalService.show(CategoriesDetailComponent,
      {
        class: 'modal-lg',
        backdrop: 'static'
      });

    this.bsModalRef.content.saved.subscribe(response => {
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
      entityId: this.selectedItems[0].data.id
    };
    this.bsModalRef = this.modalService.show(CategoriesDetailComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });
    this.bsModalRef.content.saved.subscribe((response) => {
      this.bsModalRef.hide();
      this.loadData(response.id);
    });
  }

  deleteItems() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const ids = [];
    this.selectedItems.map(res => {
      ids.push(res.data.id);
    });
    this.notificationService.showConfirmation(MessageConstants.Confirm_Delete,
      () => this.deleteItemsConfirm(ids));
  }

  deleteItemsConfirm(ids: any[]) {
    this.blockedPanel = true;
    this.categoriesService.delete(ids).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.loadData();
      this.selectedItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(MessageConstants.Delete_Failed);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    });
  }
}
