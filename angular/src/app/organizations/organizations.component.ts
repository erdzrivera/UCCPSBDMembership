import { ListService, PagedResultDto, PermissionService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { OrganizationService, OrganizationDto } from '../proxy/members';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
    selector: 'app-organizations',
    templateUrl: './organizations.component.html',
    providers: [ListService],
    standalone: false
})
export class OrganizationsComponent implements OnInit {
    organization = { items: [], totalCount: 0 } as PagedResultDto<OrganizationDto>;

    selectedOrganization = {} as OrganizationDto;
    form: FormGroup;
    isModalOpen = false;

    constructor(
        public readonly list: ListService,
        private organizationService: OrganizationService,
        private fb: FormBuilder,
        private confirmation: ConfirmationService,
        private permissionService: PermissionService
    ) { }

    get canManage(): boolean {
        return this.permissionService.getGrantedPolicy('Membership.Organizations.Edit') ||
            this.permissionService.getGrantedPolicy('Membership.Organizations.Delete');
    }

    ngOnInit() {
        const organizationStreamCreator = (query) => this.organizationService.getList(query);

        this.list.hookToQuery(organizationStreamCreator).subscribe((response) => {
            this.organization = response;
        });
    }

    createOrganization() {
        this.selectedOrganization = {} as OrganizationDto;
        this.buildForm();
        this.isModalOpen = true;
    }

    editOrganization(id: string) {
        this.organizationService.get(id).subscribe((organization) => {
            this.selectedOrganization = organization;
            this.buildForm();
            this.isModalOpen = true;
        });
    }

    buildForm() {
        this.form = this.fb.group({
            id: [this.selectedOrganization.id || ''],
            name: [this.selectedOrganization.name || '', Validators.required],
            abbreviation: [this.selectedOrganization.abbreviation || '', Validators.required],
        });
    }

    save() {
        if (this.form.invalid) {
            return;
        }

        const request = this.selectedOrganization.id
            ? this.organizationService.update(this.selectedOrganization.id, this.form.value)
            : this.organizationService.create(this.form.value);

        request.subscribe(() => {
            this.isModalOpen = false;
            this.form.reset();
            this.list.get();
        });
    }

    delete(id: string) {
        this.confirmation.warn('::Are you you want to delete?', '::Are you sure?').subscribe((status) => {
            if (status === Confirmation.Status.confirm) {
                this.organizationService.delete(id).subscribe(() => this.list.get());
            }
        });
    }
}
