import { Injectable } from '@angular/core';
import { PathLocationStrategy } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class StaticLocationStrategy extends PathLocationStrategy {
  override pushState(state: any, title: string, url: string, queryParams: string): void {
    if (this.isBaseUrl(url)) {
      super.pushState(state, title, url, queryParams);
    }
  }

  override replaceState(state: any, title: string, url: string, queryParams: string): void {
    if (this.isBaseUrl(url)) {
      super.replaceState(state, title, url, queryParams);
    }
  }

  private isBaseUrl(url: string): boolean {
    if (!url) return true;
    const cleanUrl = url.replace(/\/$/, ''); // Remove trailing slash
    return cleanUrl === '' || cleanUrl === '/UCCPSBDMembership' || cleanUrl === '/home';
  }
}
