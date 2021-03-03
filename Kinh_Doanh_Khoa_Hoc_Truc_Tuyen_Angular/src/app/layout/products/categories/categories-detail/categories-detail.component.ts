import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { MessageConstants } from '../../../../shared';
import { NotificationService } from '../../../../shared/services';
import { CategoriesService } from '../../../../shared/services/categories.service';

@Component({
  selector: 'app-categories-detail',
  templateUrl: './categories-detail.component.html',
  styleUrls: ['./categories-detail.component.scss']
})
export class CategoriesDetailComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private categoriesService: CategoriesService,
    private notificationService: NotificationService,
    private fb: FormBuilder) {
  }
  public blockedPanel = false;
  public entityForm: FormGroup;
  public dialogTitle: string;
  public entityId: string;
  public btnDisabled = false;

  saved: EventEmitter<any> = new EventEmitter();
  public rootCategories: any[] = [];

  // Validate
  noSpecial: RegExp = /^[^<>*!_~]+$/;
  validation_messages = {
    'name': [
      { type: 'required', message: 'Bạn phải nhập tên trang' },
      { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' }
    ],
    'sortOrder': [
      { type: 'required', message: 'Bạn phải nhập thứ tự' }
    ]
  };

  ngOnInit() {
    this.entityForm = this.fb.group({
      'id': new FormControl(),
      'parentId': new FormControl(),
      'name': new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(255),
        Validators.minLength(3)
      ])),
      'sortOrder': new FormControl(1, Validators.required)
    });
    if (this.entityId) {
      this.dialogTitle = 'Cập nhật';
      this.loadParents(this.entityId);
      this.loadDetail(this.entityId);
    } else {
      this.loadParents(null);
      this.dialogTitle = 'Thêm mới';
    }
  }

  loadDetail(id: any) {
    this.btnDisabled = true;
    this.blockedPanel = true;
    this.categoriesService.getDetail(id)
      .subscribe((response: any) => {
        this.entityForm.setValue({
          id: response.id,
          parentId: response.parentId,
          name: response.name,
          sortOrder: response.sortOrder
        });
        setTimeout(() => {
          this.btnDisabled = false;
          this.blockedPanel = false;
        }, 1000);
      }, () => {
        setTimeout(() => {
          this.btnDisabled = false;
          this.blockedPanel = false;
        }, 1000);
      });
  }

  loadParents(id) {
    this.categoriesService.getAllByParentId(id)
      .subscribe((response: any) => {
        this.rootCategories = [];
        response.forEach(element => {
          this.rootCategories.push({
            value: element.id,
            label: element.name
          });
        });
      });
  }

  saveChange() {
    this.btnDisabled = true;
    this.blockedPanel = true;
    if (this.entityId) {
      this.categoriesService.update(this.entityId, this.entityForm.getRawValue())
        .subscribe(() => {
          this.notificationService.showSuccess(MessageConstants.Updated_Ok);
          this.saved.emit(this.entityForm.value);
          setTimeout(() => {
            this.btnDisabled = false;
            this.blockedPanel = false;
          }, 1000);
        }, error => {
          setTimeout(() => {
            this.notificationService.showError(error);
            this.btnDisabled = false;
            this.blockedPanel = false;
          }, 1000);
        });
    } else {
      this.categoriesService.add(this.entityForm.value)
        .subscribe(() => {
          this.notificationService.showSuccess(MessageConstants.Created_Ok);
          this.saved.emit(this.entityForm.value);
          setTimeout(() => {
            this.btnDisabled = false;
            this.blockedPanel = false;
          }, 1000);
        }, () => {
          setTimeout(() => {
            this.notificationService.showError(MessageConstants.Created_Failed);
            this.btnDisabled = false;
            this.blockedPanel = false;
          }, 1000);
        });

    }
  }
}
