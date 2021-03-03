import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { Announcement } from '../../../../shared/models';
import { AnnouncementService, AuthService } from '../../../../shared/services';


@Component({
  selector: 'app-announcement-detail',
  templateUrl: './announcement-detail.component.html',
  styleUrls: ['./announcement-detail.component.scss']
})
export class AnnouncementDetailComponent implements OnInit, OnDestroy {
  constructor(
    public bsModalRef: BsModalRef,
    private authService: AuthService,
    private announcementService: AnnouncementService) {
  }
  private subscription = new Subscription();
  public announceId: string;
  public btnDisabled = false;
  public blockedPanel = false;
  public userId: string;
  public res: Announcement;
  public selectedItems = [];
  ngOnInit() {
    if (this.announceId) {
      this.loadFormDetails(this.announceId);
    }
  }
  private loadFormDetails(announceId) {
    this.blockedPanel = true;
    this.btnDisabled = true;
    const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
    this.userId = decode.sub;
    this.subscription.add(this.announcementService.getDetail(announceId, this.userId)
      .subscribe((response: Announcement) => {
        this.res = response;
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }, () => {
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }));
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}

