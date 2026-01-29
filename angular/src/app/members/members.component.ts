import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { MemberService, MemberDto } from '../proxy/members';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';

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
  isModalOpen = false;
  lastName = '';
  firstName = '';
  middleName = '';
  birthdayStart = '';
  birthdayEnd = '';

  selectedOrg = '';
  selectedType = '';
  selectedStatus = null;

  organizations = [
    { id: 'UCSCA', name: 'UCSCA' },
    { id: 'UCM', name: 'UCM' },
    { id: 'CWA', name: 'CWA' },
    { id: 'CYAF', name: 'CYAF' },
    { id: 'CYF', name: 'CYF' },
    { id: 'KIDS', name: 'KIDS' }
  ];

  memberTypes = [
    { id: '1', name: 'Regular' },
    { id: '2', name: 'Associate' },
    { id: '3', name: 'Affiliate' },
    { id: '4', name: 'Preparatory' },
    { id: '5', name: 'Honorary' }
  ];

  constructor(
    public readonly list: ListService,
    private memberService: MemberService,
    private confirmation: ConfirmationService,
    private router: Router
  ) { }

  ngOnInit() {
    this.list.hookToQuery(query => {
      if (!query.sorting) {
        query.sorting = 'lastName ASC';
      }
      return this.memberService.getList({
        ...query,
        lastName: this.lastName,
        firstName: this.firstName,
        middleName: this.middleName,
        birthdayStart: this.birthdayStart,
        birthdayEnd: this.birthdayEnd,
        organizationId: this.selectedOrg,
        memberTypeId: this.selectedType,
        isActive: this.selectedStatus
      } as any);
    }).subscribe(response => {
      this.member = response;
    });
  }

  onActivate(event) {
    if (event.type === 'click' && event.column.prop !== undefined) {
      this.router.navigate(['/members', event.row.id]);
    }
  }

  createMember() {
    this.selectedMember = {} as MemberDto;
    this.isModalOpen = true;
  }

  editMember(id: string) {
    this.selectedMember = { id } as MemberDto;
    this.isModalOpen = true;
  }

  delete(id: string) {
    this.confirmation.warn('::Are you you want to delete?', '::Are you sure?').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.memberService.delete(id).subscribe(() => this.list.get());
      }
    });
  }


  onSaved() {
    this.list.get();
  }
}
