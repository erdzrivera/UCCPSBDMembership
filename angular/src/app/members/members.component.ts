import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { MemberService, MemberDto, CreateUpdateMemberDto, MembershipTypeService, OrganizationService, MembershipTypeDto, OrganizationDto } from '../proxy/members';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss'],
  providers: [ListService],
  standalone: false
})
export class MembersComponent implements OnInit {
  member = { items: [], totalCount: 0 } as PagedResultDto<MemberDto>;

  selectedMember = {} as MemberDto;
  form: FormGroup;
  isModalOpen = false;

  membershipTypes: MembershipTypeDto[] = [];
  organizations: OrganizationDto[] = [];

  constructor(
    public readonly list: ListService,
    private memberService: MemberService,
    private membershipTypeService: MembershipTypeService,
    private organizationService: OrganizationService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) { }

  ngOnInit() {
    this.list.hookToQuery(query => this.memberService.getList(query)).subscribe(response => {
      this.member = response;
    });

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

  createMember() {
    this.selectedMember = {} as MemberDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editMember(id: string) {
    this.memberService.get(id).subscribe(member => {
      this.selectedMember = member;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      firstName: [this.selectedMember.firstName || '', Validators.required],
      middleName: [this.selectedMember.middleName || ''],
      lastName: [this.selectedMember.lastName || '', Validators.required],
      birthday: [this.selectedMember.birthday || null],
      occupation: [this.selectedMember.occupation || ''],
      baptismDate: [this.selectedMember.baptismDate || null],
      baptizedBy: [this.selectedMember.baptizedBy || ''],
      memberTypeId: [this.selectedMember.memberTypeId || null],
      organizationId: [this.selectedMember.organizationId || null],
      isActive: [this.selectedMember.isActive === false ? false : true]
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedMember.id
      ? this.memberService.update(this.selectedMember.id, this.form.value)
      : this.memberService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: string) {
    this.confirmation.warn('::Are you you want to delete?', '::Are you sure?').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.memberService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
