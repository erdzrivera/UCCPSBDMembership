import { environment } from '../environments/environment';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { Component, inject, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>

    <!-- Global Spinner Overlay -->
    <div *ngIf="isLoading" class="global-spinner-overlay">
      <div class="spinner-border text-primary" role="status" style="width: 4rem; height: 4rem;">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <abp-dynamic-layout></abp-dynamic-layout>
    <abp-internet-status></abp-internet-status>
  `,
  styles: [`
    .global-spinner-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100vw;
      height: 100vh;
      background-color: rgba(255, 255, 255, 0.7);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 9999;
    }
  `],
  standalone: false
})
export class AppComponent implements OnInit, AfterViewInit, OnDestroy {
  private authService = inject(AuthService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);
  private configState = inject(ConfigStateService);

  isLoading = false;
  private observer: MutationObserver | undefined;

  constructor() {
    this.configureOAuth();
    this.authService.init();

    // Subscribe to Router Events for Spinner
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.isLoading = true;
      }
      if (event instanceof NavigationEnd || event instanceof NavigationCancel || event instanceof NavigationError) {
        this.isLoading = false;
        this.updateSidebarVisibility();
      }
    });

    // Subscribe to User State Changes to toggle sidebar immediately upon login/logout
    this.configState.getOne$('currentUser').subscribe((user) => {
      this.updateSidebarVisibility(user);
    });
  }

  ngOnInit() {
    this.updateSidebarVisibility();

    // Setup MutationObserver to hide specific permission tabs
    this.observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.addedNodes.length) {
          this.hideUnwantedPermissionTabs();
          this.injectPageHeaderIcons();
        }
      });
    });

    this.observer.observe(document.body, {
      childList: true,
      subtree: true
    });
  }

  ngOnDestroy() {
    if (this.observer) {
      this.observer.disconnect();
    }
  }

  ngAfterViewInit() {
    this.setupSidebarToggleListener();
  }

  setupSidebarToggleListener() {
    document.addEventListener('click', (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      const toggleBtn = target.closest('.lpx-sidebar-toggle, .menu-collapse-icon, .lpx-navbar-toggler, .lpx-sidebar .lpx-icon');

      if (toggleBtn) {
        const wrapper = document.getElementById('lpx-wrapper');
        const sidebar = document.querySelector('.lpx-sidebar-container');

        if (wrapper && sidebar) {
          wrapper.classList.add('suppress-hover');
          const removeSuppression = () => {
            wrapper.classList.remove('suppress-hover');
            sidebar.removeEventListener('mouseleave', removeSuppression);
          };
          sidebar.addEventListener('mouseleave', removeSuppression);
        }
      }
    });
  }

  updateSidebarVisibility(user?: any) {
    const isAuthenticated = user?.id || this.authService.isAuthenticated;
    const isLoginPage = this.router.url === '/' && !isAuthenticated;

    if (isLoginPage) {
      document.body.classList.add('hide-navigation');
    } else {
      document.body.classList.remove('hide-navigation');
    }
  }

  private hideUnwantedPermissionTabs() {
    const hiddenTerms = ['Feature management', 'Setting management', 'Tenant management'];

    // Hide nav-links (Standard Tabs)
    const tabs = document.querySelectorAll('.nav-link');
    tabs.forEach((tab: any) => {
      const text = tab.innerText || '';
      if (hiddenTerms.some(term => text.includes(term))) {
        tab.style.display = 'none';
        tab.style.visibility = 'hidden';

        // Hide parent LI or wrapper if it exists
        const parent = tab.parentElement;
        if (parent && (parent.tagName === 'LI' || parent.classList.contains('nav-item'))) {
          parent.style.display = 'none';
        }
      }
    });

    // Hide Pills (Standard ABP sometimes uses simple items)
    const items = document.querySelectorAll('.nav-item');
    items.forEach((item: any) => {
      const text = item.innerText || '';
      if (hiddenTerms.some(term => text.includes(term))) {
        item.style.display = 'none';

        // Check for preceding separator
        const prev = item.previousElementSibling;
        if (prev && (prev.tagName === 'HR' || prev.classList.contains('dropdown-divider'))) {
          prev.style.display = 'none';
        }

        // Check for succeeding separator (sometimes used)
        const next = item.nextElementSibling;
        if (next && (next.tagName === 'HR' || next.classList.contains('dropdown-divider'))) {
          next.style.display = 'none';
        }
      }
    });

    // Fix Browser Autocomplete on New User Form
    // Chrome loves to autofill "admin" into the new user form. We force it off.
    const inputs = document.querySelectorAll('input[type="text"], input[type="password"]');
    inputs.forEach((input: any) => {
      const id = input.id || '';
      const name = input.name || '';
      // ABP Identity Modal IDs often contain 'name' or 'password'
      if (id.includes('name') || id.includes('password') || name.includes('userName') || name.includes('password')) {
        // Only apply if it doesn't have it yet to avoid constant thrashing (though cheap)
        if (input.getAttribute('autocomplete') !== 'new-password') {
          input.setAttribute('autocomplete', 'new-password');
        }
      }
    });

    this.deduplicateSeparators();
  }

  private injectPageHeaderIcons() {
    const titles = document.querySelectorAll('.lpx-page-title, h1.custom-page-title, .lpx-content-header h1, h1, h2');
    titles.forEach((title: any) => {
      const text = title.innerText.trim();
      if ((text === 'Users' || text.includes('Users')) && !title.querySelector('.fa-user')) {
        title.innerHTML = `<i class="fas fa-user me-2 text-primary"></i> ` + text;
      } else if ((text === 'Roles' || text.includes('Roles')) && !title.querySelector('.fa-user-shield')) {
        title.innerHTML = `<i class="fas fa-user-shield me-2 text-primary"></i> ` + text;
      }
    });
  }

  private deduplicateSeparators() {
    // 1. Find all potential containers (usually .nav or .flex-column inside the modal)
    // We start from the items we know we hid to find their parent
    const hiddenTerms = ['Feature management', 'Setting management', 'Tenant management'];
    const candidates = document.querySelectorAll('.nav-link, .nav-item');
    let container: HTMLElement | null = null;

    for (let i = 0; i < candidates.length; i++) {
      const text = (candidates[i] as HTMLElement).innerText || '';
      if (hiddenTerms.some(term => text.includes(term))) {
        container = candidates[i].parentElement;
        break;
      }
    }

    if (!container) return;

    // 2. Linear Scan through children to collapse separators
    const children = Array.from(container.children) as HTMLElement[];
    let lastVisibleWasSeparator = false;

    children.forEach((child) => {
      // Skip elements we explicitly hid or that use display:none
      if (child.style.display === 'none' || getComputedStyle(child).display === 'none') {
        return;
      }

      const isSeparator = child.tagName === 'HR' || child.classList.contains('dropdown-divider');

      if (isSeparator) {
        if (lastVisibleWasSeparator) {
          // Duplicate separator found (or separator at start if we init to true?)
          // Let's hide this one
          child.style.display = 'none';
        } else {
          // This is the first separator in a sequence
          lastVisibleWasSeparator = true;
        }
      } else {
        // Found actual content (a tab btn)
        lastVisibleWasSeparator = false;
      }
    });

    // Optional: Hide trailing separator if the list ends with one
    // (We iterate again or track index, but usually handled by next cycle check or just leaving it is fine 
    // unless it's at the very bottom. User said "1 line", implying between items.)
  }

  private configureOAuth() {
    if (typeof window !== 'undefined') {
      this.oauthService.setStorage(localStorage);
    }

    const config = {
      ...environment.oAuthConfig,
      strictDiscoveryDocumentValidation: false,
      requireHttps: false,
      disableAtHashCheck: true,
      skipIssuerCheck: true,
      showDebugInformation: true,
      issuer: environment.oAuthConfig.issuer
    };
    this.oauthService.configure(config);

    this.oauthService.loadDiscoveryDocument();
  }
}
