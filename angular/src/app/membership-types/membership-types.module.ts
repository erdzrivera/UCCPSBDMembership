import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MembershipTypesRoutingModule } from './membership-types-routing.module';
import { MembershipTypesComponent } from './membership-types.component';

@NgModule({
    declarations: [MembershipTypesComponent],
    imports: [
        CommonModule,
        MembershipTypesRoutingModule,
        SharedModule
    ]
})
export class MembershipTypesModule { }
