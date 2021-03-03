import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';
import { Subscription } from 'rxjs';
import { SystemConstants, MessageConstants } from '../../../shared';
import { Permission, PermissionUpdateRequest } from '../../../shared/models';
import { RolesService, CommandsService, NotificationService, UtilitiesService } from '../../../shared/services';
import { PermissionsService } from '../../../shared/services/permissions.service';

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.css']
})
export class PermissionsComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();

  public bsModalRef: BsModalRef;
  public blockedPanel = false;

  public functions: any[];
  public flattenFunctions: any[] = [];
  public selectedRole: any = {
    id: null
  };
  public roles: any[] = [];
  public commands: any[] = [];

  public selectedViews: string[] = [];
  public selectedCreates: string[] = [];
  public selectedUpdates: string[] = [];
  public selectedDeletes: string[] = [];
  public selectedExportExcel: string[] = [];
  public selectedApprove: string[] = [];


  public isSelectedAllViews = false;
  public isSelectedAllCreates = false;
  public isSelectedAllUpdates = false;
  public isSelectedAllDeletes = false;
  public isSelectedAllExportExcel = false;
  public isSelectedAllApprove = false;
  constructor(

    private permissionsService: PermissionsService,
    private rolesService: RolesService,
    private _notificationService: NotificationService,
    private _utilityService: UtilitiesService) {
  }


  ngOnInit() {
    this.loadAllRoles();
  }

  changeRole($event: any) {
    if ($event.value != null) {
      this.loadData($event.value.id);
    } else {
      this.functions = [];
    }
  }
  public reloadData() {
    this.loadData(this.selectedRole.id);
  }
  public savePermission() {
    if (this.selectedRole.id == null) {
      this._notificationService.showError('Bạn chưa chọn nhóm quyền.');
      return;
    }
    const listPermissions: Permission[] = [];
    this.selectedCreates.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.Create_Command
      });
    });
    this.selectedUpdates.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.Update_Command
      });
    });
    this.selectedDeletes.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.Delete_Command
      });
    });
    this.selectedViews.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.View_Command
      });
    });

    this.selectedExportExcel.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.ExportExcel_Command
      });
    });
    this.selectedApprove.forEach(element => {
      listPermissions.push({
        functionId: element,
        roleId: this.selectedRole.id,
        commandId: SystemConstants.Approve_Command
      });
    });
    const permissionsUpdateRequest = new PermissionUpdateRequest();
    permissionsUpdateRequest.permissions = listPermissions.filter((value, index, self) =>
    self.findIndex(t => t.functionId === value.functionId && t.commandId === value.commandId) === index);
    this.subscription.add(this.permissionsService.save(this.selectedRole.id, permissionsUpdateRequest)
      .subscribe(() => {
        this._notificationService.showSuccess(MessageConstants.Updated_Ok);

        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        this._notificationService.showError(MessageConstants.Updated_Failed);
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  loadData(roleId) {
    if (roleId != null) {
      this.blockedPanel = true;
      this.subscription.add(this.permissionsService.getFunctionWithCommands()
        .subscribe((response: any) => {
          console.log('Lay Tat Ca Permission rui nek: ' + response);
          const unflattering = this._utilityService.UnflatteringForTree(response);
          this.functions = <TreeNode[]>unflattering;
          this.flattenFunctions = response;
          this.fillPermissions(roleId);
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }, error => {
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }));
    }

  }
  checkChanged(checked: any, commandId: string, functionId: string, parentId: string) {
    if (commandId === SystemConstants.View_Command) {
      if (checked.checked) {
        this.selectedViews.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedViews.push(...childFunctions);
        } else {
          if (this.selectedViews.filter(x => x === parentId).length === 0) {
            this.selectedViews.push(parentId);
          }
        }
      } else {
        this.selectedViews = this.selectedViews.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedViews = this.selectedViews.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }

    } else if (commandId === SystemConstants.Create_Command) {
      if (checked.checked) {
        this.selectedCreates.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedCreates.push(...childFunctions);
        } else {
          if (this.selectedCreates.filter(x => x === parentId).length === 0) {
            this.selectedCreates.push(parentId);
          }
        }
      } else {
        this.selectedCreates = this.selectedCreates.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedCreates = this.selectedCreates.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }
    } else if (commandId === SystemConstants.Update_Command) {
      if (checked.checked) {
        this.selectedUpdates.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedUpdates.push(...childFunctions);
        } else {
          if (this.selectedUpdates.filter(x => x === parentId).length === 0) {
            this.selectedUpdates.push(parentId);
          }
        }
      } else {
        this.selectedUpdates = this.selectedUpdates.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedUpdates = this.selectedUpdates.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }
    } else if (commandId === SystemConstants.Delete_Command) {
      if (checked.checked) {
        this.selectedDeletes.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedDeletes.push(...childFunctions);
        } else {
          if (this.selectedDeletes.filter(x => x === parentId).length === 0) {
            this.selectedDeletes.push(parentId);
          }
        }
      } else {
        this.selectedDeletes = this.selectedDeletes.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedDeletes = this.selectedDeletes.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }
    } else if (commandId === SystemConstants.ExportExcel_Command) {
      if (checked.checked) {
        this.selectedExportExcel.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedExportExcel.push(...childFunctions);
        } else {
          if (this.selectedExportExcel.filter(x => x === parentId).length === 0) {
            this.selectedExportExcel.push(parentId);
          }
        }
      } else {
        this.selectedExportExcel = this.selectedExportExcel.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedExportExcel = this.selectedExportExcel.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }
    } else if (commandId === SystemConstants.Approve_Command) {
      if (checked.checked) {
        this.selectedApprove.push(functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedApprove.push(...childFunctions);
        } else {
          if (this.selectedApprove.filter(x => x === parentId).length === 0) {
            this.selectedApprove.push(parentId);
          }
        }
      } else {
        this.selectedApprove = this.selectedApprove.filter(x => x !== functionId);
        if (parentId === null) {
          const childFunctions = this.flattenFunctions.filter(x => x.parentId === functionId).map(x => x.id);
          this.selectedApprove = this.selectedApprove.filter(function (el) {
            return !childFunctions.includes(el);
          });
        }
      }
    }
  }
  fillPermissions(roleId: any) {
    this.blockedPanel = true;
    this.subscription.add(this.rolesService.getRolePermissions(roleId)
      .subscribe((response: Permission[]) => {
        this.selectedCreates = [];
        this.selectedUpdates = [];
        this.selectedDeletes = [];
        this.selectedViews = [];
        this.selectedExportExcel = [];
        this.selectedApprove = [];
        response.forEach(element => {
          if (element.commandId === SystemConstants.Create_Command) {
            this.selectedCreates.push(element.functionId);
          }
          if (element.commandId === SystemConstants.Update_Command) {
            this.selectedUpdates.push(element.functionId);
          }
          if (element.commandId === SystemConstants.Delete_Command) {
            this.selectedDeletes.push(element.functionId);
          }
          if (element.commandId === SystemConstants.View_Command) {
            this.selectedViews.push(element.functionId);
          }
          if (element.commandId === SystemConstants.ExportExcel_Command) {
            this.selectedExportExcel.push(element.functionId);
          }
          if (element.commandId === SystemConstants.Approve_Command) {
            this.selectedApprove.push(element.functionId);
          }
        });
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        console.log('Loi Do Data Permission' + error);
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }
  loadAllRoles() {
    this.blockedPanel = true;
    this.subscription.add(this.rolesService.getAll()
      .subscribe((response: any) => {
        this.roles = response;
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  selectAll(checked: any, uniqueCode: string) {
    if (uniqueCode === SystemConstants.View_Command) {
      this.selectedViews = [];
      // tslint:disable-next-line:triple-equals
      if (checked.checked) {
        this.selectedViews.push(...this.flattenFunctions.map(x => x.id));
      }
    } else if (uniqueCode === SystemConstants.Create_Command) {
      this.selectedCreates = [];
      if (checked.checked) {
        this.selectedCreates.push(...this.flattenFunctions.map(x => x.id));
      }
    } else if (uniqueCode === SystemConstants.Update_Command) {
      this.selectedUpdates = [];
      if (checked.checked) {
        this.selectedUpdates.push(...this.flattenFunctions.map(x => x.id));
      }
    } else if (uniqueCode === SystemConstants.Delete_Command) {
      this.selectedDeletes = [];
      if (checked.checked) {
        this.selectedDeletes.push(...this.flattenFunctions.map(x => x.id));
      }
    } else if (uniqueCode === SystemConstants.ExportExcel_Command) {
      this.selectedExportExcel = [];
      if (checked.checked) {
        this.selectedExportExcel.push(...this.flattenFunctions.map(x => x.id));
      }
    } else if (uniqueCode === SystemConstants.Approve_Command) {
      this.selectedApprove = [];
      if (checked.checked) {
        this.selectedApprove.push(...this.flattenFunctions.map(x => x.id));
      }
    }
  }



}
