import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MessageConstants } from '../../../../shared';
import { AuthService, LessonsService, NotificationService, SignalRService, UtilitiesService } from '../../../../shared/services';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-lessons-detail',
  templateUrl: './lessons-detail.component.html',
  styleUrls: ['./lessons-detail.component.scss']
})
export class LessonsDetailComponent implements OnInit, OnDestroy {

  constructor(
    private authService: AuthService,
    private lessonsService: LessonsService,
    private notificationService: NotificationService,
    private utilitiesService: UtilitiesService,
    private signalRSevice: SignalRService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder) {
  }
  public role: string;
  private subscription = new Subscription();
  public entityForm: FormGroup;
  public dialogTitle: string;
  public courseId: number;
  public lessonId: number;
  public blockedPanel = false;
  public selectedVideos: File[] = [];
  public selectedFiles: File[] = [];
  public fileVideoPath = '';
  public fileVideoName: string;
  public fileAttachPath = '';
  public fileAttachName: string;
  public backendApiUrl = environment.ApiUrl;
  public isStatus: any[] = [
    {value: 0, label : 'Chưa Duyệt'},
    {value: 1, label : 'Duyệt'},
    {value: 2, label : 'Không Duyệt'}
  ];
  // Validate
  validation_messages = {
    'name': [
      { type: 'required', message: 'Trường này bắt buộc' },
    ],
    'sortOrder': [
      { type: 'required', message: 'Trường này bắt buộc' },
      { type: 'min', message: 'Sort Order > 0'}
    ],
  };

  ngOnInit() {
    this.entityForm = this.fb.group({
      'name': new FormControl('', Validators.compose([
        Validators.required
      ])),
      'status': new FormControl(1, Validators.compose([
        Validators.required
      ])),
      'sortOrder': new FormControl(0, Validators.compose([
        Validators.required,
        Validators.min(0)])),
      'id': new FormControl(0),
      'courseId': new FormControl(0)
    });
    this.subscription.add(this.activeRoute.params.subscribe(params => {
      this.courseId = params['coursesId'];
      this.lessonId = params['id'];
    }));
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.role = decode.role;
    if (this.lessonId) {
      this.loadFormDetails(this.lessonId);
      this.dialogTitle = 'Cập nhật';
    } else {
      this.dialogTitle = 'Thêm mới';
    }
  }

  private loadFormDetails(id: any) {
    this.blockedPanel = true;
    this.subscription.add(this.lessonsService.getDetail(id).subscribe((response: any) => {
      this.entityForm.setValue({
        name: response.name,
        courseId: this.courseId,
        sortOrder: response.sortOrder,
        status: response.status,
        id: response.id
      });
      this.fileVideoPath = response.videoPath;
      this.fileVideoName = response.videoPath.substring(response.videoPath.lastIndexOf('/') + 1);
      this.fileAttachPath = response.attachment;
      this.fileAttachName = response.attachment.substring(response.attachment.lastIndexOf('/') + 1);
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
  public deleteAttachment() {
    this.blockedPanel = true;
    this.subscription.add(this.lessonsService.deleteAttachment(this.courseId, this.lessonId)
      .subscribe((res: any) => {
        if (res.StatusCode === 200) {
          this.notificationService.showSuccess(MessageConstants.Delete_Ok);
          this.fileAttachName = '';
          this.fileAttachPath = '';
        }
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        this.notificationService.showError(MessageConstants.Delete_Failed);
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }));
  }

  public selectVideo($event) {
    if ($event.currentFiles) {
      $event.currentFiles.forEach(element => {
        this.selectedVideos.push(element);
      });
    }
  }
  public removeVideo($event) {
    if ($event.file) {
      this.selectedVideos.splice(this.selectedVideos.findIndex(item => item.name === $event.file.name), 1);
    }

  }
  public changeVideo() {
    this.fileVideoName = '';
    this.fileVideoPath = '';
    // this.blockedPanel = true;
    // this.subscription.add(this.lessonsService.deleteVideo(this.courseId, this.lessonId)
    //   .subscribe((res: any) => {
    //     console.log('Video Delete nek1: ' + res);
    //     console.log('Video StatusCode Delete nek2: ' + res.StatusCode);
    //     console.log('Video status Delete nek3: ' + res.status);
    //     if (res.StatusCode === 200) {
    //       this.notificationService.showSuccess(MessageConstants.Delete_Ok);
    //       this.fileVideoName = '';
    //       this.fileVideoPath = '';
    //     }
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }, error => {
    //     this.notificationService.showError(MessageConstants.Delete_Failed);
    //     setTimeout(() => { this.blockedPanel = false; }, 1000);
    //   }));
  }
  goBackToList() {
    this.router.navigateByUrl('/products/courses/' + this.courseId + '/lessons');
  }
  public saveChange() {
    this.blockedPanel = true;
    const formValues = this.entityForm.getRawValue();
    formValues.courseId = this.courseId;
    if (this.role === 'Teacher') {
      formValues.status = false;
    }
    const formData = this.utilitiesService.ToFormData(formValues);
    this.selectedFiles.forEach(file => {
      formData.append('attachment', file, file.name);
    });
    this.selectedVideos.forEach(file => {
      formData.append('videoPath', file, file.name);
    });
    if (this.lessonId) {
      this.subscription.add(this.lessonsService.update(this.lessonId, formData)
        .subscribe((response: any) => {
          if (response.status === 200 && response.body) {
            if (response.body.announcementViewModel) {
              this.signalRSevice.SendMessageToUser('SendToUserAsync', response.body.userId, response.body.announcementViewModel);
            }
            this.notificationService.showSuccess(MessageConstants.Updated_Ok);
            this.router.navigateByUrl('/products/courses/' + this.courseId + '/lessons');
          }
        }, error => {
          this.notificationService.showError(error);
          setTimeout(() => { this.blockedPanel = false; }, 1000);
        }));
    } else {
      this.subscription.add(this.lessonsService.add(formData)
        .subscribe((response: any) => {
          if (response.status === 200) {
            this.notificationService.showSuccess(MessageConstants.Created_Ok);
            this.router.navigateByUrl('/products/courses/' + this.courseId + '/lessons');
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

