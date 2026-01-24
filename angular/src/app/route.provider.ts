import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/members',
        name: '::Members',
        iconClass: 'fas fa-users',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'Membership.Members',
      },
      {
        path: '/organizations',
        name: '::Organizations',
        iconClass: 'fas fa-building',
        order: 3,
        layout: eLayoutType.application,
        requiredPolicy: 'Membership.Organizations',
      },
      {
        path: '/membership-types',
        name: '::Membership Types',
        iconClass: 'fas fa-list',
        order: 4,
        layout: eLayoutType.application,
        requiredPolicy: 'Membership.MembershipTypes',
      },
    ]);
  };
}
