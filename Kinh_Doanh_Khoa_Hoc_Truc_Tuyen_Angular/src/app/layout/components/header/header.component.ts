import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SignalRService } from '../../../shared/services/signal-r.service';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../shared/services/auth.service';
import { AnnouncementService, NotificationService } from '../../../shared/services';
import { Announcement, AnnouncementMarkReadRequest } from '../../../shared/models';


@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
    public pushRightClass: string;
    private subscription = new Subscription();
    userName: string;
    public avatar: string;
    public userId: string;
    public pageIndex = 1;
    public pageSize = 3;
    public backendApiUrl = environment.ApiUrl;
    public totalUnred = 0;
    public announcement: Announcement[];
    constructor(
        private translate: TranslateService,
        public router: Router,
        public announcementService: AnnouncementService,
        private signalRSevice: SignalRService,
        private authService: AuthService) {
        const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
        this.userName = decode.preferred_username;
        this.avatar = this.backendApiUrl + decode.Avatar;
        this.userId = decode.sub;
        this.router.events.subscribe((val) => {
            if (val instanceof NavigationEnd && window.innerWidth <= 992 && this.isToggled()) {
                this.toggleSidebar();
            }
        });
        // Realtime
        this.signalRSevice.retrieveMappedObject().subscribe(
            (message) => {
                this.loadAnnouncement();
            });
    }
    ngOnDestroy(): void {
       this.subscription.unsubscribe();
    }

    ngOnInit() {
        this.pushRightClass = 'push-right';
        this.loadAnnouncement();
    }

    isToggled(): boolean {
        const dom: Element = document.querySelector('body');
        return dom.classList.contains(this.pushRightClass);
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    rltAndLtr() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle('rtl');
    }

    async signout() {
        await this.authService.signOut();
    }

    changeLang(language: string) {
        this.translate.use(language);
    }

    loadAnnouncement() {
        this.subscription.add(this.announcementService.getAllPaging('false', this.userId, this.pageIndex, this.pageSize)
        .subscribe(async (res) => {
            if (res) {
                this.announcement = res.items;
                this.totalUnred = res.totalRecords;
            }
        }));
    }
    markAsRead(announceId: string) {
        const announce = new AnnouncementMarkReadRequest();
        announce.announceId = announceId;
        announce.userId = this.userId;
        this.subscription.add(this.announcementService.updateMaskAsRead(announce).subscribe(async (response: any) => {
            if (response) {
                this.loadAnnouncement();
            }
        }, error => {
            console.log(error);
        }));
    }
}
