import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberService } from '@proxy/members';
import { MemberDto } from '@proxy/members/models';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
    selector: 'app-view-member',
    templateUrl: './view-member.component.html',
    styleUrls: ['./view-member.component.scss'],
    standalone: false
})
export class ViewMemberComponent implements OnInit {
    member: MemberDto;
    isModalOpen = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private memberService: MemberService,
        private confirmation: ConfirmationService
    ) { }

    ngOnInit(): void {
        this.getMember();
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
        });
    }

    onEdit() {
        this.isModalOpen = true;
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
}
