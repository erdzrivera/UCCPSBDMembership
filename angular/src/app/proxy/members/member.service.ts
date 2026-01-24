import { RestService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { CreateUpdateMemberDto, MemberDto, MembershipTypeDto, OrganizationDto } from './models';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MemberService {
    apiName = 'Default';

    constructor(private restService: RestService) { }

    get(id: string): Observable<MemberDto> {
        return this.restService.request(
            {
                method: 'GET',
                url: `/api/app/member/${id}`,
            },
            { apiName: this.apiName }
        );
    }

    getList(input: PagedAndSortedResultRequestDto): Observable<PagedResultDto<MemberDto>> {
        return this.restService.request(
            {
                method: 'GET',
                url: '/api/app/member',
                params: input,
            },
            { apiName: this.apiName }
        );
    }

    create(input: CreateUpdateMemberDto): Observable<MemberDto> {
        return this.restService.request(
            {
                method: 'POST',
                url: '/api/app/member',
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    update(id: string, input: CreateUpdateMemberDto): Observable<MemberDto> {
        return this.restService.request(
            {
                method: 'PUT',
                url: `/api/app/member/${id}`,
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    delete(id: string): Observable<void> {
        return this.restService.request(
            {
                method: 'DELETE',
                url: `/api/app/member/${id}`,
            },
            { apiName: this.apiName }
        );
    }
}

@Injectable({
    providedIn: 'root',
})
export class MembershipTypeService {
    apiName = 'Default';

    constructor(private restService: RestService) { }

    get(id: string): Observable<MembershipTypeDto> {
        return this.restService.request(
            {
                method: 'GET',
                url: `/api/app/membership-type/${id}`,
            },
            { apiName: this.apiName }
        );
    }

    getList(input: PagedAndSortedResultRequestDto): Observable<PagedResultDto<MembershipTypeDto>> {
        return this.restService.request(
            {
                method: 'GET',
                url: '/api/app/membership-type',
                params: input,
            },
            { apiName: this.apiName }
        );
    }

    create(input: MembershipTypeDto): Observable<MembershipTypeDto> {
        return this.restService.request(
            {
                method: 'POST',
                url: '/api/app/membership-type',
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    update(id: string, input: MembershipTypeDto): Observable<MembershipTypeDto> {
        return this.restService.request(
            {
                method: 'PUT',
                url: `/api/app/membership-type/${id}`,
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    delete(id: string): Observable<void> {
        return this.restService.request(
            {
                method: 'DELETE',
                url: `/api/app/membership-type/${id}`,
            },
            { apiName: this.apiName }
        );
    }
}

@Injectable({
    providedIn: 'root',
})
export class OrganizationService {
    apiName = 'Default';

    constructor(private restService: RestService) { }

    get(id: string): Observable<OrganizationDto> {
        return this.restService.request(
            {
                method: 'GET',
                url: `/api/app/organization/${id}`,
            },
            { apiName: this.apiName }
        );
    }

    getList(input: PagedAndSortedResultRequestDto): Observable<PagedResultDto<OrganizationDto>> {
        return this.restService.request(
            {
                method: 'GET',
                url: '/api/app/organization',
                params: input,
            },
            { apiName: this.apiName }
        );
    }

    create(input: OrganizationDto): Observable<OrganizationDto> {
        return this.restService.request(
            {
                method: 'POST',
                url: '/api/app/organization',
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    update(id: string, input: OrganizationDto): Observable<OrganizationDto> {
        return this.restService.request(
            {
                method: 'PUT',
                url: `/api/app/organization/${id}`,
                body: input,
            },
            { apiName: this.apiName }
        );
    }

    delete(id: string): Observable<void> {
        return this.restService.request(
            {
                method: 'DELETE',
                url: `/api/app/organization/${id}`,
            },
            { apiName: this.apiName }
        );
    }
}
