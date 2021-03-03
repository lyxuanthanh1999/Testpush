import { FunctionsComponent } from './functions/functions.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AuthGuard } from '../../shared';

const routes: Routes = [
    {
        path: '',
        component: UsersComponent,
        data: {
            functionCode: 'User'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'users',
        component: UsersComponent,
        data: {
            functionCode: 'User'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'functions',
        component: FunctionsComponent,
        data: {
            functionCode: 'Function'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'roles',
        component: RolesComponent,
        data: {
            functionCode: 'Role'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'permissions',
        component: PermissionsComponent,
        data: {
            functionCode: 'Permission'
        },
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SystemsRoutingModule { }
