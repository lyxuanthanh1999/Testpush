import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RolesService } from '../../../../shared/services/roles.service';
import { NotificationService } from '../../../../shared/services';
import { MessageConstants } from '../../../../shared';

@Component({
  selector: 'app-roles-detail',
  templateUrl: './roles-detail.component.html',
  styleUrls: ['./roles-detail.component.scss']
})
export class RolesDetailComponent implements OnInit, OnDestroy {
  constructor(
    public bsModalRef: BsModalRef,
    private rolesService: RolesService,
    private notificationService: NotificationService,
    private fb: FormBuilder) {
  }

  private subscription = new Subscription();
  public entityForm: FormGroup;
  public dialogTitle: string;
  private savedEvent: EventEmitter<any> = new EventEmitter();
  public entityId: string;
  public btnDisabled = false;

  public blockedPanel = false;

  // Validate
  validation_messages = {
    'name': [
      { type: 'required', message: 'Trường này bắt buộc' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 30 kí tự' }
    ]
  };

  ngOnInit() {
    this.entityForm = this.fb.group({
      'name': new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50)
      ]))
    });
    if (this.entityId) {
      this.dialogTitle = 'Cập nhật';
      this.loadFormDetails(this.entityId);
    } else {
      this.dialogTitle = 'Thêm mới';
    }
  }

  private loadFormDetails(id: any) {
    this.blockedPanel = true;
    this.subscription.add(this.rolesService.getDetail(id).subscribe((response: any) => {
      this.entityForm.setValue({
     //   id: response.id,
        name: response.name
      });
      setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
    }, error => {
      setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
    }));
  }
  public saveChange() {
    this.btnDisabled = true;
    this.blockedPanel = true;
    if (this.entityId) {
      this.subscription.add(this.rolesService.update(this.entityId, this.entityForm.getRawValue().name)
        .subscribe(() => {
          this.savedEvent.emit(this.entityForm.value);
          this.notificationService.showSuccess(MessageConstants.Updated_Ok);
          this.btnDisabled = false;
          setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
        }, error => {
          this.notificationService.showError(error);
          setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
        }));
    } else {
      this.subscription.add(this.rolesService.add(this.entityForm.getRawValue().name)
        .subscribe(() => {
          this.savedEvent.emit(this.entityForm.value);
          this.notificationService.showSuccess(MessageConstants.Created_Ok);
          this.btnDisabled = false;
          setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
        }, error => {
          this.notificationService.showError(error);
          setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
        }));
    }
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
