import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MemberService, MemberDto, MembershipTypeService, OrganizationService, MembershipTypeDto, OrganizationDto, CreateUpdateMemberDto } from '@proxy/members';

@Component({
    selector: 'app-member-modal',
    templateUrl: './member-modal.component.html',
    standalone: false
})
export class MemberModalComponent implements OnInit {
    @Input() visible = false;
    @Output() visibleChange = new EventEmitter<boolean>();

    @Input() memberId: string;
    @Output() saved = new EventEmitter<void>();

    form: FormGroup;
    membershipTypes: MembershipTypeDto[] = [];
    organizations: OrganizationDto[] = [];

    constructor(
        private fb: FormBuilder,
        private memberService: MemberService,
        private membershipTypeService: MembershipTypeService,
        private organizationService: OrganizationService
    ) { }

    ngOnInit(): void {
        this.getDropdownLists();
    }

    ngOnChanges(): void {
        if (this.visible) {
            if (this.memberId) {
                this.memberService.get(this.memberId).subscribe(member => {
                    this.buildForm(member);
                });
            } else {
                this.buildForm({} as MemberDto);
            }
        }
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
            birthday: [selectedMember.birthday || null],
            occupation: [selectedMember.occupation || ''],
            baptismDate: [selectedMember.baptismDate || null],
            baptizedBy: [selectedMember.baptizedBy || ''],
            memberTypeId: [selectedMember.memberTypeId || null],
            organizationId: [selectedMember.organizationId || null],
            placeOfBirth: [selectedMember.placeOfBirth || ''],
            fatherName: [selectedMember.fatherName || ''],
            motherName: [selectedMember.motherName || ''],
            sponsors: [selectedMember.sponsors || ''],
            isActive: [selectedMember.isActive === false ? false : true]
        });
    }

    save() {
        if (this.form.invalid) {
            return;
        }

        const request = this.memberId
            ? this.memberService.update(this.memberId, this.form.value)
            : this.memberService.create(this.form.value);

        request.subscribe(() => {
            this.visible = false;
            this.visibleChange.emit(this.visible);
            this.saved.emit();
            this.form.reset();
        });
    }

    close() {
        this.visible = false;
        this.visibleChange.emit(this.visible);
    }
}
