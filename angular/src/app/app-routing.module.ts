import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
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
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
    canActivate: [AuthGuard]
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
    canActivate: [AuthGuard]
  },
  { path: 'members', loadChildren: () => import('./members/members.module').then(m => m.MembersModule), canActivate: [AuthGuard] },
  { path: 'organizations', loadChildren: () => import('./organizations/organizations.module').then(m => m.OrganizationsModule), canActivate: [AuthGuard] },
  { path: 'membership-types', loadChildren: () => import('./membership-types/membership-types.module').then(m => m.MembershipTypesModule), canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule { }
