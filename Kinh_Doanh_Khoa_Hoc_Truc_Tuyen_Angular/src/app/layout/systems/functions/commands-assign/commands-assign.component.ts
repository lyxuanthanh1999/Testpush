import { Component, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { CommandAssign } from '../../../../shared/models';
import { FunctionsService, CommandsService, NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-commands-assign',
  templateUrl: './commands-assign.component.html',
  styleUrls: ['./commands-assign.component.scss']
})
export class CommandsAssignComponent implements OnInit, OnDestroy {
  public blockedPanel = false;
  public items: any[];
  public selectedItems: any[] = [];
  public dialogTitle: string;
  public functionId: string;
  public existingCommands: any[] = [];
  public addToAllFunctions = false;
  private subscription = new Subscription();
  private chosenEvent: EventEmitter<any> = new EventEmitter();
  constructor(
    public bsModalRef: BsModalRef,
    private functionsService: FunctionsService,
    private commandsService: CommandsService) {
  }
  ngOnDestroy(): void {
  this.subscription.unsubscribe();
  }

  ngOnInit() {
    this.loadAllCommands();
  }

  loadAllCommands() {
    this.blockedPanel = true;
    this.subscription.add(this.commandsService.getAll()
      .subscribe((response: any) => {
        this.items = [];

        const existingCommands = this.existingCommands;
        const notExistingCommands = response.filter(function (item) {
          return existingCommands.indexOf(item.id) === -1;
        });

        this.items = notExistingCommands;
        if (this.selectedItems.length === 0 && this.items.length > 0) {
          this.selectedItems.push(this.items[0]);
        }
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }


  chooseCommands() {
    this.blockedPanel = true;
    const selectedItemIds = [];
    this.selectedItems.forEach(element => {
      selectedItemIds.push(element.id);
    });
    const entity = new CommandAssign();
    entity.addToAllFunctions = this.addToAllFunctions;
    entity.commandIds = selectedItemIds;

    this.subscription.add(this.functionsService.addCommandsToFunction(this.functionId, entity).subscribe(() => {
      this.chosenEvent.emit(this.selectedItems);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
}
