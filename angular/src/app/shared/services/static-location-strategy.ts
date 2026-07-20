import { Injectable } from '@angular/core';
import { PathLocationStrategy } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class StaticLocationStrategy extends PathLocationStrategy {
  override pushState(state: any, title: string, url: string, queryParams: string): void {
    // Do nothing: prevent browser history / address bar from updating
  }

  override replaceState(state: any, title: string, url: string, queryParams: string): void {
    // Do nothing: prevent browser history / address bar from updating
  }
}
