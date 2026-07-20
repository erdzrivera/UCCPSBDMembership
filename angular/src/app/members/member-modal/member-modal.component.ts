import { Component, EventEmitter, Input, OnInit, Output, OnChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MemberService, MemberDto, MembershipTypeService, OrganizationService, MembershipTypeDto, OrganizationDto, CreateUpdateMemberDto } from '@proxy/members';

@Component({
    selector: 'app-member-modal',
    templateUrl: './member-modal.component.html',
    standalone: false
})
export class MemberModalComponent implements OnInit, OnChanges {
    @Input() visible = false;
    @Output() visibleChange = new EventEmitter<boolean>();

    @Input() memberId: string;
    @Output() saved = new EventEmitter<void>();

    form: FormGroup;
    membershipTypes: MembershipTypeDto[] = [];
    organizations: OrganizationDto[] = [];
    activeTab = 'personal';
    today = new Date().toISOString().split('T')[0];

    noFutureDateValidator(control: any) {
        if (!control.value) {
            return null;
        }
        const selectedDate = new Date(control.value);
        const todayDate = new Date();
        todayDate.setHours(0, 0, 0, 0);
        return selectedDate > todayDate ? { futureDate: true } : null;
    }

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
            this.activeTab = 'personal';
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
            birthday: [selectedMember.birthday || null, [Validators.required, this.noFutureDateValidator.bind(this)]],
            occupation: [selectedMember.occupation || ''],
            baptismDate: [selectedMember.baptismDate || null],
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

    nextTab() {
        if (this.activeTab === 'personal') {
            this.activeTab = 'family';
        } else if (this.activeTab === 'family') {
            this.activeTab = 'baptism';
        }
    }

    prevTab() {
        if (this.activeTab === 'family') {
            this.activeTab = 'personal';
        } else if (this.activeTab === 'baptism') {
            this.activeTab = 'family';
        }
    }

    isTabValid(tab: string): boolean {
        if (!this.form) return false;
        if (tab === 'personal') {
            return (
                (this.form.get('firstName')?.valid ?? false) &&
                (this.form.get('lastName')?.valid ?? false) &&
                (this.form.get('organizationId')?.valid ?? false) &&
                (this.form.get('memberTypeId')?.valid ?? false)
            );
        }
        if (tab === 'family') {
            return this.form.get('birthday')?.valid ?? false;
        }
        return true;
    }
}
