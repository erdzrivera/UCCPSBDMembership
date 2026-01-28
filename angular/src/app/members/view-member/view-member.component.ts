import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberService } from '@proxy/members';
import { MemberDto } from '@proxy/members/models';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MembershipTypeService, OrganizationService, MembershipTypeDto, OrganizationDto } from '@proxy/members';

@Component({
    selector: 'app-view-member',
    templateUrl: './view-member.component.html',
    styleUrls: ['./view-member.component.scss'],
    standalone: false
})
export class ViewMemberComponent implements OnInit {
    member: MemberDto;
    isModalOpen = false;
    activeTab = 'personal';
    isEditing = false;
    form: FormGroup;
    membershipTypes: MembershipTypeDto[] = [];
    organizations: OrganizationDto[] = [];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private memberService: MemberService,
        private confirmation: ConfirmationService,
        private fb: FormBuilder,
        private membershipTypeService: MembershipTypeService,
        private organizationService: OrganizationService
    ) { }

    ngOnInit(): void {
        this.getMember();
        this.getDropdownLists();
    }

    getDropdownLists() {
        this.membershipTypeService.getList({} as any).subscribe(res => {
            this.membershipTypes = res.items;
        });
        this.organizationService.getList({} as any).subscribe(res => {
            this.organizations = res.items;
        });
    }

    buildForm(selectedMember: MemberDto) {
        this.form = this.fb.group({
            firstName: [selectedMember.firstName || '', Validators.required],
            middleName: [selectedMember.middleName || ''],
            lastName: [selectedMember.lastName || '', Validators.required],
            birthday: [selectedMember.birthday ? new Date(selectedMember.birthday).toISOString().substring(0, 10) : null, Validators.required],
            occupation: [selectedMember.occupation || ''],
            baptismDate: [selectedMember.baptismDate ? new Date(selectedMember.baptismDate).toISOString().substring(0, 10) : null],
            baptizedBy: [selectedMember.baptizedBy || ''],
            memberTypeId: [selectedMember.memberTypeId || null, Validators.required],
            organizationId: [selectedMember.organizationId || null, Validators.required],
            placeOfBirth: [selectedMember.placeOfBirth || ''],
            fatherName: [selectedMember.fatherName || ''],
            motherName: [selectedMember.motherName || ''],
            sponsors: [selectedMember.sponsors || ''],
            isActive: [selectedMember.isActive === false ? false : true]
        });
    }

    private isValid(value: string | null | undefined): boolean {
        if (!value) return false;
        const v = value.trim().toLowerCase();
        return v !== 'n/a' && v !== 'na' && v !== 'none' && v !== 'missing';
    }

    get canGenerateCertificate(): boolean {
        if (!this.member) return false;
        // Require minimal set of fields for a valid certificate
        return (
            !!this.member.firstName &&
            !!this.member.lastName &&
            !!this.member.baptismDate &&
            this.isValid(this.member.placeOfBirth) &&
            this.isValid(this.member.fatherName) &&
            this.isValid(this.member.motherName) &&
            this.isValid(this.member.baptizedBy) &&
            this.isValid(this.member.sponsors)
        );
    }

    get certificateValidationMessage(): string {
        if (!this.member) return '';
        const missing = [];
        if (!this.member.baptismDate) missing.push('Baptism Date');
        if (!this.isValid(this.member.placeOfBirth)) missing.push('Place of Birth');
        if (!this.isValid(this.member.fatherName)) missing.push('Father\'s Name');
        if (!this.isValid(this.member.motherName)) missing.push('Mother\'s Name');
        if (!this.isValid(this.member.baptizedBy)) missing.push('Officiating Minister');
        if (!this.isValid(this.member.sponsors)) missing.push('Sponsors');

        if (missing.length > 0) {
            return `Missing or Invalid information: ${missing.join(', ')}`;
        }
        return '';
    }

    getMember() {
        const id = this.route.snapshot.params.id;
        this.memberService.get(id).subscribe(res => {
            this.member = res;
            this.buildForm(res);
        });
    }

    onEdit() {
        this.isEditing = true;
    }

    onCancel() {
        this.isEditing = false;
        this.buildForm(this.member);
    }

    onSave() {
        if (this.form.invalid) {
            return;
        }

        this.memberService.update(this.member.id, this.form.value).subscribe(() => {
            this.isEditing = false;
            this.getMember();
        });
    }

    onDelete() {
        this.confirmation.warn('::Are you sure you want to delete this member?', '::Are you sure?').subscribe((status) => {
            if (status === Confirmation.Status.confirm) {
                this.memberService.delete(this.member.id).subscribe(() => {
                    this.router.navigate(['/members']);
                });
            }
        });
    }

    onSaved() {
        this.getMember();
    }

    getOrganizationName(identifier: string): string {
        if (!identifier) return 'None';
        const org = this.organizations.find(o => o.id === identifier || o.abbreviation === identifier);
        if (!org) return identifier;
        return org.abbreviation ? `${org.name} (${org.abbreviation})` : org.name;
    }

    getMemberTypeName(identifier: string): string {
        if (!identifier) return 'None';
        const type = this.membershipTypes.find(t => t.id === identifier || t.name === identifier);
        return type ? type.name : identifier;
    }
}
