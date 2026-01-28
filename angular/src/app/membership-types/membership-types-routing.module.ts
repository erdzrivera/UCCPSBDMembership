import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MembershipTypesComponent } from './membership-types.component';

const routes: Routes = [{ path: '', component: MembershipTypesComponent, title: '::Membership Types' }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MembershipTypesRoutingModule { }
