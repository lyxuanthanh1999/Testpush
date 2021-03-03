import { MessageConstants } from './../../../../shared';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { Category } from '../../../../shared/models';
import { CoursesService, CategoriesService, NotificationService, UtilitiesService, AuthService, SignalRService } from '../../../../shared/services';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-courses-detail',
  templateUrl: './courses-detail.component.html',
  styleUrls: ['./courses-detail.component.scss']
})
export class CoursesDetailComponent implements OnInit, OnDestroy {

  constructor(
    private coursesService: CoursesService,
    private authService: AuthService,
    private categoriesService: CategoriesService,
    private signalRSevice: SignalRService,
    private notificationService: NotificationService,
    private utilitiesService: UtilitiesService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder) {
  }
  public role: string;
  private subscription = new Subscription();
  public entityForm: FormGroup;
  public dialogTitle: string;
  public entityId: number;
  public categories: SelectItem[] = [];
  public blockedPanel = false;
  public selectedFiles: File[] = [];
  public filePath = '';
  public fileName: string;
  public backendApiUrl = environment.ApiUrl;
  public isStatus: any[] = [
    {value: 1, label : 'Phát Hành'},
    {value: 2, label : 'Chưa Phát Hành'},
    {value: 3, label : 'Ngừng Kinh Doanh'}];
  // Validate
  validation_messages = {
    'name': [
      { type: 'required', message: 'Trường này bắt buộc' },
    ],
    'categoryId': [
      { type: 'required', message: 'Trường này bắt buộc' },
    ],
    'status': [
      { type: 'required', message: 'Trường này bắt buộc' },
    ],
    'price': [
      { type: 'required', message: 'Trường này bắt buộc' },
      { type: 'min', message: 'Số tiền phải lớn hơn 5000' },
    ]
  };

  ngOnInit() {
    this.entityForm = this.fb.group({
      'categoryId': new FormControl('', Validators.compose([
        Validators.required
      ])),
      'name': new FormControl('', Validators.compose([
        Validators.required
      ])),
      'status': new FormControl(1, Validators.compose([
        Validators.required
      ])),
      'price': new FormControl(0, Validators.compose([
        Validators.required,
        Validators.min(5000)])),
      'description': new FormControl(''),
      'content': new FormControl(''),
      'id': new FormControl(0),
    });
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.role = decode.role;
    this.subscription.add(this.activeRoute.params.subscribe(params => {
      this.entityId = params['coursesId'];
    }));

    this.subscription.add(this.categoriesService.getAll()
      .subscribe((response: Category[]) => {
        response.forEach(element => {
          this.categories.push({ label: element.name, value: element.id });
        });

        if (this.entityId) {
          this.loadFormDetails(this.entityId);
          this.dialogTitle = 'Cập nhật';
        } else {
          this.dialogTitle = 'Thêm mới';
        }
      }));
  }

  private loadFormDetails(id: any) {
    this.blockedPanel = true;
    this.subscription.add(this.coursesService.getDetail(id).subscribe((response: any) => {
      this.entityForm.setValue({
        name: response.name,
        categoryId: response.categoryId,
        description: response.description,
        content: response.content,
        price: response.price,
        status: response.status,
        id: response.id
      });
      this.filePath = response.image;
      this.fileName = response.image.substring(response.image.lastIndexOf('/') + 1);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }, error => {
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    }));
  }
  public selectAttachments($event) {
    if ($event.currentFiles) {
      $event.currentFiles.forEach(element => {
        this.selectedFiles.push(element);
      });
    }
  }
  public removeAttachments($event) {
    if ($event.file) {
      this.selectedFiles.splice(this.selectedFiles.findIndex(item => item.name === $event.file.name), 1);
    }

  }
  public changeAttachment() {
    this.fileName = '';
    this.filePath = '';
    // this.blockedPanel = true;
    // this.subscription.add(this.coursesService.deleteAttachment(this.entityId)
    //   .subscribe((res: any) => {
    //     console.log('Delete nek1: ' + res);
    //     console.log('StatusCode Delete nek2: ' + res.StatusCode);
    //     console.log('status Delete nek3: ' + res.status);
    //     if (res.StatusCode === 200) {
    //       this.notificationService.showSuccess(MessageConstants.Delete_Ok);
    //       this.fileName = '';
    //       this.filePath = '';
    //     }
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }, error => {
    //     this.notificationService.showError(MessageConstants.Delete_Failed);
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }));
  }
  goBackToList() {
    this.router.navigateByUrl('/products/courses');
  }
  public saveChange() {
    this.blockedPanel = true;
    const formValues = this.entityForm.getRawValue();
    if (this.role === 'Teacher') {
      formValues.status = 2;
    }
    const formData = this.utilitiesService.ToFormData(formValues);
    this.selectedFiles.forEach(file => {
      formData.append('image', file, file.name);
    });

    if (this.entityId) {
      this.subscription.add(this.coursesService.update(this.entityId, formData)
        .subscribe((response: any) => {
          console.log(response);
          if (response.status === 200 && response.body) {
            if (response.body.announcementViewModel) {
              this.signalRSevice.SendMessageToUser('SendToUserAsync', response.body.userId, response.body.announcementViewModel);
            }
            this.notificationService.showSuccess(MessageConstants.Updated_Ok);
            this.router.navigateByUrl('/products/courses');
          }
        }, error => {
          this.notificationService.showError(error);
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }));
    } else {
      this.subscription.add(this.coursesService.add(formData)
        .subscribe((response: any) => {
          if (response.status === 200) {
            this.notificationService.showSuccess(MessageConstants.Created_Ok);
            this.router.navigateByUrl('/products/courses');
          }
        }, error => {
          this.notificationService.showError(error);
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }));
    }
    return false;
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
