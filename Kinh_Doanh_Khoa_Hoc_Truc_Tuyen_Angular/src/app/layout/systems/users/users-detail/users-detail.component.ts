import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { MessageConstants } from '../../../../shared';
import { UsersService, NotificationService, UtilitiesService } from '../../../../shared/services';

@Component({
  selector: 'app-users-detail',
  templateUrl: './users-detail.component.html',
  styleUrls: ['./users-detail.component.scss']
})
export class UsersDetailComponent implements OnInit, OnDestroy {

  constructor(
    public bsModalRef: BsModalRef,
    private usersService: UsersService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private utilitiesService: UtilitiesService,
    private datePipe: DatePipe
    ) {}
public blockedPanel = false;
public myRoles: string[] = [];
public entityForm: FormGroup;
public dialogTitle: string;
public entityId: string;
private subscription = new Subscription();
public btnDisabled = false;
public saveBtnName: string;
public closeBtnName: string;
public vi: any;
saved: EventEmitter<any> = new EventEmitter();

public selectedAvatar: File[] = [];
public fileAvatarPath = '';
public fileAvatarName: string;
public backendApiUrl = environment.ApiUrl;
// Validate
noSpecial: RegExp = /^[^<>*!_~]+$/;
validation_messages = {
    'name': [
        { type: 'required', message: 'Bạn phải nhập tên người dùng' },
        { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' }
    ],
    'userName': [
        { type: 'required', message: 'Bạn phải nhập tên tài khoản' },
        { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' }
    ],
    'password': [
        { type: 'required', message: 'Bạn phải nhập tên tài khoản' },
        { type: 'minlength', message: 'Bạn phải nhập ít nhất 4 kí tự' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
    ],
    'email': [
        { type: 'required', message: 'Bạn phải nhập email' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
        { type: 'pattern', message: 'Bạn phải nhập đúng định dạng Email' }
    ],
    'phoneNumber': [
        { type: 'required', message: 'Bạn phải nhập số điện thoại' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 12 kí tự' },
    ],
    'dob': [
        { type: 'required', message: 'Bạn phải nhập ngày sinh' }
    ]
};

ngOnInit() {
    this.entityForm = this.fb.group({
        'id': new FormControl(''),
        'name': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(255),
            Validators.minLength(3)
        ])),
        'biography': new FormControl(),
        'userName': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(255),
            Validators.minLength(3)
        ])),
        'password': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(255),
            Validators.minLength(4)
        ])),
        'email': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(255),
            Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
        ])),
        'phoneNumber': new FormControl('', Validators.compose([
            Validators.required,
            Validators.maxLength(12)
        ])),
        'dob': new FormControl('', Validators.compose([
            Validators.required]))
    });
    if (this.entityId) {
        this.loadUserDetail(this.entityId);
        this.dialogTitle = 'Cập nhật';
        this.entityForm.controls['userName'].disable({ onlySelf: true });
        this.entityForm.controls['password'].disable({ onlySelf: true });
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
ngOnDestroy(): void {
    this.subscription.unsubscribe();
}
loadUserDetail(id: any) {
    this.btnDisabled = true;
    this.blockedPanel = true;
    this.subscription.add(this.usersService.getDetail(id)
        .subscribe((response: any) => {
            const dob: Date = new Date(response.dob);
            this.entityForm.setValue({
                id: response.id,
                name: response.name,
                biography: response.biography,
                userName: response.userName,
                email: response.email,
                password: '',
                phoneNumber: response.phoneNumber,
                dob: dob
            });
            this.fileAvatarPath = response.avatar;
            this.fileAvatarName = response.avatar.substring(response.avatar.lastIndexOf('/') + 1);
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

public selectAvatar($event) {
    if ($event.currentFiles) {
      $event.currentFiles.forEach(element => {
        this.selectedAvatar.push(element);
      });
    }
  }
  public removeAvatar($event) {
    if ($event.file) {
      this.selectedAvatar.splice(this.selectedAvatar.findIndex(item => item.name === $event.file.name), 1);
    }
  }
  public changeAvatar() {
    this.fileAvatarName = '';
    this.fileAvatarPath = '';
    // this.blockedPanel = true;
    // this.subscription.add(this.usersService.deleteAvatar(this.entityId)
    //   .subscribe((res: any) => {
    //     console.log('Avatar Delete nek1: ' + res);
    //     console.log('Avatar StatusCode Delete nek2: ' + res.StatusCode);
    //     console.log('Avatar status Delete nek3: ' + res.status);
    //     if (res.StatusCode === 200) {
    //       this.notificationService.showSuccess(MessageConstants.Delete_Ok);
    //       this.fileAvatarName = '';
    //       this.fileAvatarPath = '';
    //     }
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }, () => {
    //     this.notificationService.showError(MessageConstants.Delete_Failed);
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }));
  }

saveChange() {
    this.btnDisabled = true;
    this.blockedPanel = true;
    const rawValues = this.entityForm.getRawValue();
     rawValues.dob = this.datePipe.transform(this.entityForm.controls['dob'].value, 'MM/dd/yyyy');
     const formData = this.utilitiesService.ToFormData(rawValues);
     this.selectedAvatar.forEach(file => {
        formData.append('avatar', file, file.name);
      });
    if (this.entityId) {
        this.subscription.add(this.usersService.update(this.entityId, formData)
            .subscribe((res: any) => {
                if (res.status === 204) {
                    setTimeout(() => {
                        this.btnDisabled = false;
                        this.blockedPanel = false;
                    }, 1000);
                    this.notificationService.showSuccess(MessageConstants.Updated_Ok);
                    this.saved.emit(this.entityForm.value);
                }
            }, error => {
                this.notificationService.showError(error);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
            }));
    } else {
        this.subscription.add(this.usersService.add(formData)
            .subscribe((res: any) => {
             if (res.status === 200) {
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
                this.saved.emit(this.entityForm.value);
             }
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
