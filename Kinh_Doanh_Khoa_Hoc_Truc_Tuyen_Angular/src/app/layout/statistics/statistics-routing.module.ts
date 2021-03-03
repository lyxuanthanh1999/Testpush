import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RevenueComponent } from './revenue/revenue.component';
import { NewUserComponent } from './new-user/new-user.component';
import { AuthGuard } from '../../shared';


const routes: Routes = [
    {
        path: '',
        component: RevenueComponent
    },
    {
        path: 'revenue',
        component: RevenueComponent,
        data: {
            functionCode: 'Revenue'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'new-user',
        component: NewUserComponent,
        data: {
            functionCode: 'NewUser'
        },
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StatisticsRoutingModule { }
