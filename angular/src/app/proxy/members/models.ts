import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';

export interface MemberDto {
  id: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  birthday?: string;
  occupation?: string;
  baptismDate?: string;
  baptizedBy?: string;
  placeOfBirth?: string;
  fatherName?: string;
  motherName?: string;
  sponsors?: string;
  memberTypeId?: string;
  organizationId?: string;
  isActive: boolean;
}

export interface CreateUpdateMemberDto {
  firstName: string;
  middleName?: string;
  lastName: string;
  birthday?: string;
  occupation?: string;
  baptismDate?: string;
  baptizedBy?: string;
  memberTypeId?: string;
  organizationId?: string;
  isActive: boolean;
}

export interface MembershipTypeDto {
  id: string;
  name: string;
  description: string;
}

export interface OrganizationDto {
  id: string;
  name: string;
  abbreviation: string;
}
