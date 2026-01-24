import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { MembersRoutingModule } from './members-routing.module';
import { MembersComponent } from './members.component';

@NgModule({
  declarations: [
    MembersComponent
  ],
  imports: [
    CommonModule,
    MembersRoutingModule,
    SharedModule
  ]
})
export class MembersModule { }
