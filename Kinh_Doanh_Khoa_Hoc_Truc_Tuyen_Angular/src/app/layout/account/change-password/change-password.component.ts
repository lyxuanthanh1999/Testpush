import { DatePipe } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../shared';
import { UserChangePassword } from '../../../shared/models';
import { AuthService, UsersService, NotificationService, UtilitiesService } from '../../../shared/services';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit, OnDestroy {

  constructor(
    private authService: AuthService,
    private usersService: UsersService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder) {}
  private subscription = new Subscription();
  public dialogTitle = 'Thay đổi mật khẩu';
  public userId: string;
  public userName: string;
  public btnDisabled = false;
  public saveBtnName: string;
  public blockedPanel = false;
  public entityForm: FormGroup;
  // Validate
  noSpecial: RegExp = /^[^<>*!_~]+$/;
  validation_messages = {
    'currentPassword': [
      { type: 'required', message: 'Bạn phải nhập tên tài khoản' },
      { type: 'minlength', message: 'Bạn phải nhập ít nhất 4 kí tự' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
    ],
      'newPassword': [
        { type: 'required', message: 'Bạn phải nhập tên tài khoản' },
        { type: 'minlength', message: 'Bạn phải nhập ít nhất 4 kí tự' },
        { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' }
    ]
  };

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
  const pass = group.get('newPassword').value;
  const confirmPass = group.get('confirmPassword').value;
  return pass === confirmPass ? null : 'Xác nhận mật khẩu và mật khẩu không chính xác';
}

  ngOnInit() {
    this.entityForm = this.fb.group({
      'id': new FormControl(''),
      'userName': new FormControl(''),
      'currentPassword': new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(255),
        Validators.minLength(4)
      ])),
      'newPassword': new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(255),
        Validators.minLength(4)
      ])),
      'confirmPassword': new FormControl('')}, { validator: this.checkPasswords });
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.userId = decode.sub;
    this.userName = decode.preferred_username;
    this.entityForm.setValue({
      id: decode.sub,
      userName: decode.preferred_username,
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    });
    this.entityForm.controls['userName'].disable({ onlySelf: true });
  }
  ngOnDestroy(): void {
      this.subscription.unsubscribe();
    }

  public saveChange() {
      this.btnDisabled = true;
      this.blockedPanel = true;
      const rawValues = this.entityForm.getRawValue();
       const entity = new UserChangePassword();
       entity.id = rawValues.id;
       entity.currentPassword = rawValues.currentPassword;
       entity.newPassword = rawValues.newPassword;
      this.subscription.add(this.usersService.updatePassword(entity)
      .subscribe(() => {
        setTimeout(() => {
          this.btnDisabled = false;
          this.blockedPanel = false;
          this.notificationService.showSuccess(MessageConstants.Updated_Ok);
          this.router.navigate(['/dashboard']);
        }, 1000);
      }, error => {
          setTimeout(() => {
              this.btnDisabled = false;
              this.blockedPanel = false;
              this.notificationService.showError(error);
          }, 1000);
      }));
  }
}
