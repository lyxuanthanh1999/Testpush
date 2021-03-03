import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'prefix' },
            {
                path: 'dashboard',
                data: {
                    functionCode: 'DashBoard'
                },
                canActivate: [AuthGuard],
                loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule)
            },
            {
                path: 'systems',
                data: {
                    functionCode: 'System'
                },
                canActivate: [AuthGuard],
                loadChildren: () => import('./systems/systems.module').then((m) => m.SystemsModule)
            },
            {
                path: 'products',
                data: {
                    functionCode: 'Products'
                },
                canActivate: [AuthGuard],
                loadChildren: () => import('./products/products.module').then((m) => m.ProductsModule)
            },
            {
                path: 'statistics',
                data: {
                    functionCode: 'Statistics'
                },
                canActivate: [AuthGuard],
                loadChildren: () => import('./statistics/statistics.module').then((m) => m.StatisticsModule)
            },
            {
                path: 'account',
                loadChildren: () => import('./account/account.module').then((m) => m.AccountModule)
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule {}
