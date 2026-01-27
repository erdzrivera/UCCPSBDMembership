import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { MembersRoutingModule } from './members-routing.module';
import { MembersComponent } from './members.component';
import { ViewMemberComponent } from './view-member/view-member.component';
import { MemberModalComponent } from './member-modal/member-modal.component';
import { CertificateComponent } from './certificate/certificate.component';

@NgModule({
  declarations: [
    MembersComponent,
    ViewMemberComponent,
    MemberModalComponent,
    CertificateComponent
  ],
  imports: [
    CommonModule,
    MembersRoutingModule,
    SharedModule
  ]
})
export class MembersModule { }
