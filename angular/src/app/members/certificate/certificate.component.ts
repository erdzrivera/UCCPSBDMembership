import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MemberService } from '@proxy/members';
import { MemberDto } from '@proxy/members/models';

@Component({
    selector: 'app-certificate',
    templateUrl: './certificate.component.html',
    styleUrls: ['./certificate.component.scss'],
    standalone: false,
    encapsulation: ViewEncapsulation.None
})
export class CertificateComponent implements OnInit {
    member: MemberDto;

    constructor(
        private route: ActivatedRoute,
        private memberService: MemberService
    ) { }

    ngOnInit(): void {
        const id = this.route.snapshot.params.id;
        this.memberService.get(id).subscribe(res => {
            this.member = res;
        });
    }

    print(): void {
        const originalTitle = document.title;
        if (this.member) {
            const fileName = `${this.member.lastName}_${this.member.firstName}_certificate`;
            document.title = fileName;
        }

        window.print();

        // Restore title after a short delay to ensure print dialog picks it up
        setTimeout(() => {
            document.title = originalTitle;
        }, 1000);
    }

    get sponsorsList(): string[] {
        if (!this.member?.sponsors) return [];
        return this.member.sponsors.split('\n').map(s => s.trim()).filter(s => s.length > 0);
    }
}
