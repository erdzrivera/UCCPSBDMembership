import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MembersComponent } from './members.component';

import { ViewMemberComponent } from './view-member/view-member.component';
import { CertificateComponent } from './certificate/certificate.component';

const routes: Routes = [
  { path: '', component: MembersComponent, title: '::Members' },
  { path: ':id', component: ViewMemberComponent, data: { name: '::Members', title: '::Member Details' } },
  { path: ':id/certificate', component: CertificateComponent, data: { name: '::Members', title: 'Certificate' } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MembersRoutingModule { }
