import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../shared';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AnnouncementComponent } from './announcement/announcement.component';


const routes: Routes = [
    {
        path: '',
        component: AccountDetailComponent
    },
    {
        path: 'account-detail',
        component: AccountDetailComponent
    },
    {
        path: 'change-password',
        component: ChangePasswordComponent
    },
    {
        path: 'announcement',
        component: AnnouncementComponent
    },
    {
        path: 'announcement',
        component: AnnouncementComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AccountRoutingModule { }
