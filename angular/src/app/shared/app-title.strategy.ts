import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterStateSnapshot, TitleStrategy } from '@angular/router';

@Injectable()
export class AppTitleStrategy extends TitleStrategy {
    constructor(private readonly title: Title) {
        super();
    }

    override updateTitle(snapshot: RouterStateSnapshot): void {
        let title = this.buildTitle(snapshot);
        if (title) {
            // Remove "AbpIdentity::", "::", or any localized prefix
            title = title.replace(/.*::/, '');
            this.title.setTitle(`${title} | UCCP SBD Membership`);
        } else {
            this.title.setTitle('UCCP SBD Membership');
        }
    }
}
