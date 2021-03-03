import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { AuthService, NotificationService, UsersService, UtilitiesService } from '../../../shared/services';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.scss']
})
export class AccountDetailComponent implements OnInit, OnDestroy {

    constructor(
      private authService: AuthService,
      private usersService: UsersService,
      private notificationService: NotificationService,
      private utilitiesService: UtilitiesService,
      private datePipe: DatePipe,
      private fb: FormBuilder) {}
    private subscription = new Subscription();
    public entityForm: FormGroup;
    public dialogTitle = 'Thông tin cá nhân';
    public entityId: string;
    public userId: string;
    public btnDisabled = false;
    public saveBtnName: string;
    public vi: any;
    public blockedPanel = false;
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
        'email': [
            { type: 'required', message: 'Bạn phải nhập email' },
            { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
            { type: 'pattern', message: 'Bạn phải nhập đúng định dạng Email' }
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
            'email': new FormControl('', Validators.compose([
                Validators.required,
                Validators.maxLength(255),
                Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
            ])),
            'phoneNumber': new FormControl(),
            'dob': new FormControl('', Validators.compose([
                Validators.required]))
        });
      const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
      this.userId = decode.sub;
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
      this.loadFormDetails(this.userId);
      this.entityForm.controls['userName'].disable({ onlySelf: true });
    }
    ngOnDestroy(): void {
        this.subscription.unsubscribe();
      }

    private loadFormDetails(id: any) {
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
                    phoneNumber: response.phoneNumber,
                    dob: dob
                });
                this.fileAvatarPath = response.avatar;
                this.fileAvatarName = response.avatar.substring(response.avatar.lastIndexOf('/') + 1);
                setTimeout(() => {
                    this.btnDisabled = false;
                    this.blockedPanel = false;
                }, 1000);
            }, () => {
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
      this.blockedPanel = true;
      this.subscription.add(this.usersService.deleteAvatar(this.userId)
        .subscribe((res: any) => {
          console.log('Avatar Delete nek1: ' + res);
          console.log('Avatar StatusCode Delete nek2: ' + res.StatusCode);
          console.log('Avatar status Delete nek3: ' + res.status);
          if (res.StatusCode === 200) {
            this.notificationService.showSuccess(MessageConstants.Delete_Ok);
            this.fileAvatarName = '';
            this.fileAvatarPath = '';
          }
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }, () => {
          this.notificationService.showError(MessageConstants.Delete_Failed);
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }));
    }

    public saveChange() {
        this.btnDisabled = true;
        this.blockedPanel = true;
        const rawValues = this.entityForm.getRawValue();
         rawValues.dob = this.datePipe.transform(this.entityForm.controls['dob'].value, 'MM/dd/yyyy');
         const formData = this.utilitiesService.ToFormData(rawValues);
         this.selectedAvatar.forEach(file => {
          formData.append('avatar', file, file.name);
        });
          this.subscription.add(this.usersService.update(this.userId, formData)
          .subscribe((res: any) => {
              if (res.status === 204) {
                  setTimeout(() => {
                      this.btnDisabled = false;
                      this.blockedPanel = false;
                      this.notificationService.showSuccess(MessageConstants.Updated_Ok);
                      this.loadFormDetails(this.userId);
                  }, 1000);
              }
          }, error => {
              setTimeout(() => {
                  this.btnDisabled = false;
                  this.blockedPanel = false;
                  this.notificationService.showError(error);
              }, 1000);
          }));
    }
}
