import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';
import { MessageConstants } from '../../../shared';
import { CommandAssign } from '../../../shared/models';
import { FunctionsService, NotificationService, UtilitiesService } from '../../../shared/services';
import { CommandsAssignComponent } from './commands-assign/commands-assign.component';
import { FunctionsDetailComponent } from './functions-detail/functions-detail.component';
import { map } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-functions',
  templateUrl: './functions.component.html',
  styleUrls: ['./functions.component.css']
})
export class FunctionsComponent implements OnInit, OnDestroy {

  private subscription = new Subscription();
  private indexSelect: Number = 0;

  public bsModalRef: BsModalRef;
  public blockedPanel = false;
  public blockedPanelCommand = false;
  public showCommandGrid = false;
  // -----------------Function-----------------
  public items: TreeNode[] = [];
  public selectedItems = [];
  public prevSelectItems = [];
  // ---------------Command------------------------------
  public commands: any[] = [];
  public selectedCommandItems = [];

  constructor(
    private modalService: BsModalService,
    private functionsService: FunctionsService,
    private notificationService: NotificationService,
    private utilitiesService: UtilitiesService) {
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();

  }

  ngOnInit() {
    this.loadData();
  }


  togglePanel() {
    if (this.showCommandGrid) {
      if (this.selectedItems.length === 1) {
        this.loadDataCommand();
      }
    }

  }

  loadData(selectionId = null) {
    this.blockedPanel = true;
    this.subscription.add(this.functionsService.getAll()
      .subscribe((response: any) => {
        const functionTree = this.utilitiesService.UnflatteringForTree(response);
        this.items = <TreeNode[]>functionTree;
        if (this.selectedItems.length === 0 && this.items.length > 0) {
          this.selectedItems.push(this.items[0]);
          this.loadDataCommand();
        }
        // Nếu có là sửa thì chọn selection theo Id
        if (selectionId != null && this.items.length > 0) {
          this.selectedItems = this.items.filter(x => x.data.id === selectionId);
        }
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }


  nodeSelect(event: any) {
    this.selectedCommandItems = [];
   this.commands = [];
    if (this.selectedItems.length === 1 && this.showCommandGrid) {
      this.loadDataCommand();
    }
  }

  nodeUnSelect(event: any) {
    this.selectedCommandItems = [];
    this.commands = [];
    if (this.selectedItems.length === 1 && this.showCommandGrid) {
      this.loadDataCommand();
    }
  }

  showAddModal() {
    this.bsModalRef = this.modalService.show(FunctionsDetailComponent,
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
    this.bsModalRef = this.modalService.show(FunctionsDetailComponent,
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
    this.subscription.add(this.functionsService.delete(ids).subscribe(() => {
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.loadData();
      this.selectedItems = [];
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      this.notificationService.showError(MessageConstants.Delete_Failed);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  loadDataCommand() {
    this.blockedPanelCommand = true;
    this.subscription.add(this.functionsService.getAllCommandsByFunctionId(this.selectedItems[0].data.id)
      .subscribe((response: any) => {

        this.commands = response;
        if (this.selectedCommandItems.length === 0 && this.commands.length > 0) {
          this.selectedCommandItems.push(this.commands[0]);
        }
        this.blockedPanelCommand = false;
      }, error => {
        this.blockedPanelCommand = false;
      }));
  }

  removeCommands() {
    const selectedCommandIds = [];
    this.selectedCommandItems.forEach(element => {
      selectedCommandIds.push(element.id);
    });
    this.notificationService.showConfirmation(MessageConstants.Confirm_Delete,
      () => this.removeCommandsConfirm(selectedCommandIds));
  }

  removeCommandsConfirm(ids: string[]) {
    this.blockedPanelCommand = true;
    const entity = new CommandAssign();
    entity.commandIds = ids;
    this.subscription.add(this.functionsService.deleteCommandsFromFunction(this.selectedItems[0].data.id, entity).subscribe(() => {
      this.loadDataCommand();
      this.selectedCommandItems = [];
      this.notificationService.showSuccess(MessageConstants.Delete_Ok);
      this.blockedPanelCommand = false;
    }, error => {
      this.notificationService.showError(MessageConstants.Delete_Failed);
      this.blockedPanelCommand = false;
    }));
  }

  addCommandsToFunction() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError(MessageConstants.Not_Choose_Any_Record);
      return;
    }
    const initialState = {
      existingCommands: this.commands.map(x => x.id),
      functionId: this.selectedItems[0].data.id
    };
    this.bsModalRef = this.modalService.show(CommandsAssignComponent,
      {
        initialState: initialState,
        class: 'modal-lg',
        backdrop: 'static'
      });
    this.bsModalRef.content.chosenEvent.subscribe((response: any[]) => {
      this.bsModalRef.hide();
      this.loadDataCommand();
      this.notificationService.showSuccess(MessageConstants.Created_Ok);
      this.selectedCommandItems = [];
    }, error => {
      this.notificationService.showError(MessageConstants.Created_Failed);
    });
  }
}
