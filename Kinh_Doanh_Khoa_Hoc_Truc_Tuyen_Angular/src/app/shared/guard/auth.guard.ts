import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { SystemConstants } from '../constants';
import { AuthService } from '../services';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.authService.isAuthenticated()) {
            const functionCode = route.data['functionCode'] as string;
            const decode = this.authService.getDecodedAccessToken(this.authService.getToken());
            const permissions = JSON.parse(decode.permissions);
            const role = decode.role;
            if (role !== 'Student'
                && permissions
                && permissions.filter(x => x === functionCode + '_' + SystemConstants.View_Command).length > 0) {
                return true;
            } else {
              this.router.navigate(['/access-denied'], {
                queryParams: { redirect: state.url }
              });
              return false;
            }
        }
        this.router.navigate(['/login'], {queryParams: {redirect: state.url }, replaceUrl: true});
        return false;
    }
}
