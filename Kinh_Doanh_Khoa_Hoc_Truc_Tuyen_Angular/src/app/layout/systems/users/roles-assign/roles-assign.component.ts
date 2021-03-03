import { Component, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { UsersService, RolesService } from '../../../../shared/services';

@Component({
  selector: 'app-roles-assign',
  templateUrl: './roles-assign.component.html',
  styleUrls: ['./roles-assign.component.scss']
})
export class RolesAssignComponent implements OnInit, OnDestroy {
  // Default
  private chosenEvent: EventEmitter<any> = new EventEmitter();
  public blockedPanel = false;
  // User Role
  public items: any[];
  public selectedItems = [];
  public title: string;
  public userId: string;
  public existingRoles: any[];
  private subscription = new Subscription();
  constructor(
    public bsModalRef: BsModalRef,
    private usersService: UsersService,
    private rolesService: RolesService) {
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit() {
    this.loadAllRoles();
  }

  loadAllRoles() {
    this.blockedPanel = true;
    this.rolesService.getAll().subscribe((response: any) => {
      response.forEach(function (element) {
        element.Selected = false;
      });
      const existingRoles = this.existingRoles;
      const newRoles = response.filter(function (item) {
        return existingRoles.indexOf(item.name) === -1;
      });
      this.items = newRoles;
      if (this.selectedItems.length === 0 && this.items.length > 0) {
        this.selectedItems.push(this.items[0]);
      }
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    });
  }

  chooseRoles() {
    this.blockedPanel = true;
    const selectedNames = [];
    this.selectedItems.forEach(element => {
      selectedNames.push(element.name);
    });
    const assignRolesToUser = {
      roleNames: selectedNames
    };
    this.subscription.add(this.usersService.assignRolesToUser(this.userId, assignRolesToUser).subscribe(() => {
      this.chosenEvent.emit(this.selectedItems);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
}
