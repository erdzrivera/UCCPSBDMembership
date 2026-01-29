import { ListService, PagedResultDto, PermissionService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { MembershipTypeService, MembershipTypeDto } from '../proxy/members';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
    selector: 'app-membership-types',
    templateUrl: './membership-types.component.html',
    providers: [ListService],
    standalone: false
})
export class MembershipTypesComponent implements OnInit {
    membershipType = { items: [], totalCount: 0 } as PagedResultDto<MembershipTypeDto>;

    selectedMembershipType = {} as MembershipTypeDto;
    form: FormGroup;
    isModalOpen = false;

    constructor(
        public readonly list: ListService,
        private membershipTypeService: MembershipTypeService,
        private fb: FormBuilder,
        private confirmation: ConfirmationService,
        private permissionService: PermissionService
    ) { }

    get canManage(): boolean {
        return this.permissionService.getGrantedPolicy('Membership.MembershipTypes.Edit') ||
            this.permissionService.getGrantedPolicy('Membership.MembershipTypes.Delete');
    }

    ngOnInit() {
        const membershipTypeStreamCreator = (query) => {
            if (!query.sorting) {
                query.sorting = 'id ASC';
            }
            return this.membershipTypeService.getList(query);
        };

        this.list.hookToQuery(membershipTypeStreamCreator).subscribe((response) => {
            this.membershipType = response;
        });
    }

    createMembershipType() {
        this.selectedMembershipType = {} as MembershipTypeDto;
        this.buildForm();
        this.isModalOpen = true;
    }

    editMembershipType(id: string) {
        this.membershipTypeService.get(id).subscribe((membershipType) => {
            this.selectedMembershipType = membershipType;
            this.buildForm();
            this.isModalOpen = true;
        });
    }

    buildForm() {
        this.form = this.fb.group({
            id: [this.selectedMembershipType.id || ''],
            name: [this.selectedMembershipType.name || '', Validators.required],
            description: [this.selectedMembershipType.description || ''],
        });
    }

    save() {
        if (this.form.invalid) {
            return;
        }

        const request = this.selectedMembershipType.id
            ? this.membershipTypeService.update(this.selectedMembershipType.id, this.form.value)
            : this.membershipTypeService.create(this.form.value);

        request.subscribe(() => {
            this.isModalOpen = false;
            this.form.reset();
            this.list.get();
        });
    }

    delete(id: string) {
        this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
            if (status === Confirmation.Status.confirm) {
                this.membershipTypeService.delete(id).subscribe(() => this.list.get());
            }
        });
    }
}
