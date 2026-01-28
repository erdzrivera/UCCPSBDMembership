import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@abp/ng.core';
import { ItemPermissionGuard } from './guards/item-permission.guard';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { MyProfileComponent } from './my-profile/my-profile.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account/manage',
    component: MyProfileComponent,
    canActivate: [AuthGuard],
    title: 'My Profile'
  },
  {
    path: 'account/manage-profile',
    component: MyProfileComponent,
    canActivate: [AuthGuard],
    title: 'My Profile'
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
    canActivate: [AuthGuard]
  },
  {
    path: 'access-denied',
    component: AccessDeniedComponent
  },
  {
    path: 'members',
    loadChildren: () => import('./members/members.module').then(m => m.MembersModule),
    canActivate: [AuthGuard, ItemPermissionGuard],
    data: { requiredPolicy: 'Membership.Members' }
  },
  {
    path: 'organizations',
    loadChildren: () => import('./organizations/organizations.module').then(m => m.OrganizationsModule),
    canActivate: [AuthGuard, ItemPermissionGuard],
    data: { requiredPolicy: 'Membership.Organizations' }
  },
  {
    path: 'membership-types',
    loadChildren: () => import('./membership-types/membership-types.module').then(m => m.MembershipTypesModule),
    canActivate: [AuthGuard, ItemPermissionGuard],
    data: { requiredPolicy: 'Membership.MembershipTypes' }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule { }
