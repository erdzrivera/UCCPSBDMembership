import { inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { PermissionService } from '@abp/ng.core';
import { map, tap } from 'rxjs/operators';

export const ItemPermissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const permissionService = inject(PermissionService);
    const router = inject(Router);
    const requiredPolicy = route.data['requiredPolicy'] as string;

    if (!requiredPolicy) {
        return true;
    }

    if (!permissionService.getGrantedPolicy(requiredPolicy)) {
        router.navigate(['/access-denied']);
        return false;
    }

    return true;
};
