import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, ValidatorFn } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { MessageConstants } from '../../../../shared';
import { PromotionsService, NotificationService } from '../../../../shared/services';
import { PromotionsRequest } from '../../../../shared/models/promotions-request.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-promotions-detail',
  templateUrl: './promotions-detail.component.html',
  styleUrls: ['./promotions-detail.component.scss']
})
export class PromotionsDetailComponent implements OnInit, OnDestroy {

  constructor(
    public bsModalRef: BsModalRef,
    private promotionsService: PromotionsService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private datePipe: DatePipe
    ) {}
public blockedPanel = false;
public myRoles: string[] = [];
public entityForm: FormGroup;
public dialogTitle: string;
public entityId: string;
public isHiddenAmount = false;
public isHiddenPercent = false;
public method: any[] = [
    {value: null, label: 'Phương thức giảm giá'},
    {value: 2, label : 'Giảm theo giá'},
    {value: 1, label : 'Giảm theo %'}];
public isActive: any[] = [
        {value: true, label : 'Hoạt động'},
        {value: false, label : 'Không hoạt động'}];
public btnDisabled = false;
public saveBtnName: string;
public closeBtnName: string;
public vi: any;
private subscription = new Subscription();
saved: EventEmitter<any> = new EventEmitter();
// Validate
noSpecial: RegExp = /^[^<>*!_~]+$/;
validation_messages = {
    'name': [
        { type: 'required', message: 'Bạn phải nhập tiêu đề sự kiện' },
        { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' }
    ],
    'fromDate': [
        { type: 'required', message: 'Bạn phải nhập form date' },
    ],
    'toDate': [
        { type: 'required', message: 'Bạn phải nhập to date' },
    ],
    'chooseDiscount': [
        { type: 'required', message: 'Bạn phải nhập chọn phương thức giảm giá'}
    ],
    'discountPercent': [
        { type: 'required', message: 'Bạn phải nhập phần trăm giảm giá'},
        { type: 'min', message: 'Bạn phải nhập > 0' },
        { type: 'max', message: 'Bạn không được nhập > 100' }
    ],
    'discountAmount': [
        { type: 'required', message: 'Bạn phải nhập số tiền giảm giá'},
        { type: 'min', message: 'Bạn phải nhập >= 0' },
    ]
};
ngOnDestroy(): void {
 this.subscription.unsubscribe();
}
ngOnInit() {
    this.entityForm = this.fb.group({
        'id': new FormControl(0),
        'name': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(255),
            Validators.minLength(3)
        ])),
        'fromDate': new FormControl('', Validators.compose([
            Validators.required
        ])),
        'toDate': new FormControl('', Validators.compose([
            Validators.required
        ])),
        'chooseDiscount': new FormControl('', Validators.compose([
            Validators.required
        ])),
        'discountPercent': new FormControl('0'),
        'discountAmount': new FormControl('0'),
        'content': new FormControl(''),
        'status': new FormControl('')
    });
    if (this.entityId) {
        this.loadPromotionDetail(this.entityId);
        this.dialogTitle = 'Cập nhật';
    } else {
        this.dialogTitle = 'Thêm mới';
    }

    this.vi = {
        firstDayOfWeek: 0,
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        today: 'Today',
        clear: 'Clear'
    };
}
onChangeDiscount(event: any) {
    if (event.value === 1) {
        this.isHiddenPercent = true;
        this.isHiddenAmount = false;
        this.entityForm.controls['discountPercent'].setValidators([
            Validators.max(100),
            Validators.min(0),
            Validators.required
           ]);
        this.entityForm.controls['discountPercent'].updateValueAndValidity();
        this.entityForm.controls['discountAmount'].clearValidators();
        this.entityForm.controls['discountAmount'].updateValueAndValidity();
    } else if (event.value === 2) {
        this.isHiddenPercent = false;
        this.isHiddenAmount = true;
        this.entityForm.controls['discountAmount'].setValidators([
            Validators.min(0),
            Validators.required
            ]);
        this.entityForm.controls['discountAmount'].updateValueAndValidity();
        this.entityForm.controls['discountPercent'].clearValidators();
        this.entityForm.controls['discountPercent'].updateValueAndValidity();
    } else {
        this.isHiddenPercent = false;
        this.isHiddenAmount = false;
        this.entityForm.controls['discountAmount'].clearValidators();
        this.entityForm.controls['discountAmount'].updateValueAndValidity();
        this.entityForm.controls['discountPercent'].clearValidators();
        this.entityForm.controls['discountPercent'].updateValueAndValidity();
    }
}
loadPromotionDetail(id: any) {
    this.btnDisabled = true;
    this.blockedPanel = true;
    this.subscription.add(this.promotionsService.getDetail(id)
        .subscribe((response: any) => {
            const toDate: Date = new Date(response.toDate);
            const fromDate: Date = new Date(response.fromDate);
            let check = null;
            if (response.discountAmount !== null) {
                check = 2;
                this.isHiddenAmount = true;
            } else {
                check = 1;
                this.isHiddenPercent = true;
            }
            this.entityForm.setValue({
                id: response.id,
                name: response.name,
                discountAmount: response.discountAmount,
                discountPercent: response.discountPercent,
                toDate: toDate,
                fromDate: fromDate,
                chooseDiscount: check,
                status: response.status,
                content: response.content
            });
            setTimeout(() => {
                this.btnDisabled = false;
                this.blockedPanel = false;
            }, 1000);
        }, error => {
            setTimeout(() => {
                this.btnDisabled = false;
                this.blockedPanel = false;
            }, 1000);
        }));
}


saveChange() {
    this.btnDisabled = true;
    this.blockedPanel = true;
    const rawValues = this.entityForm.getRawValue();
     rawValues.fromDate = this.datePipe.transform(this.entityForm.controls['fromDate'].value, 'MM/dd/yyyy');
     rawValues.toDate = this.datePipe.transform(this.entityForm.controls['toDate'].value, 'MM/dd/yyyy');
     const promotion = new PromotionsRequest();
        promotion.fromDate = rawValues.fromDate.toString();
        promotion.toDate = rawValues.toDate.toString();
        promotion.name = rawValues.name;
        promotion.content = rawValues.content;
        promotion.applyForAll = false;
     if (rawValues.chooseDiscount === 1) {
        promotion.discountPercent = rawValues.discountPercent;
     } else {
        promotion.discountAmount = rawValues.discountAmount;
     }
    if (this.entityId) {
        promotion.id = Number(this.entityId);
        promotion.status = rawValues.status;
        this.subscription.add(this.promotionsService.update(promotion)
            .subscribe(() => {
                this.notificationService.showSuccess(MessageConstants.Updated_Ok);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
                this.saved.emit(this.entityForm.value);
            }, error => {
                this.notificationService.showError(error);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
            }));
    } else {
        promotion.status = rawValues.status === '' ? true : false;
        this.subscription.add(this.promotionsService.add(promotion)
            .subscribe(() => {
                this.notificationService.showSuccess(MessageConstants.Created_Ok);
                this.saved.emit(this.entityForm.value);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
            }, error => {
                this.notificationService.showError(error);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
            }));

    }
}
}
