import { environment } from '../environments/environment';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';

@Component({
  standalone: false,
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    
    <!-- Global Spinner Overlay -->
    <div *ngIf="isLoading" class="global-spinner-overlay">
      <div class="spinner-border text-primary" role="status" style="width: 4rem; height: 4rem;">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <abp-dynamic-layout />
    <abp-internet-status />
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
  `]
})
export class AppComponent implements OnInit {
  private authService = inject(AuthService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);
  private configState = inject(ConfigStateService);

  isLoading = false;

  constructor() {
    this.configureOAuth();
    this.authService.init();

    // Subscribe to Router Events for Spinner
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
  }

  // No changes to constructor or ngOnInit from plan, but need ngAfterViewInit
  ngAfterViewInit() {
    this.setupSidebarToggleListener();
  }

  setupSidebarToggleListener() {
    // We need to attach a listener to the toggle button. 
    // Since LeptonX structure might be dynamic, we delegate from body or look for specific elements.
    // The main toggle usually has class .lpx-sidebar-toggle or .menu-collapse-icon

    document.addEventListener('click', (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      // Check if click is on a toggle button or its children
      const toggleBtn = target.closest('.lpx-sidebar-toggle, .menu-collapse-icon, .lpx-navbar-toggler, .lpx-sidebar .lpx-icon');

      // We only care if the sidebar is currently Expanded (pinned) and we are about to Collapse it.
      // But finding the exact state from DOM can be tricky.
      // Easiest is to ALWAYS add 'suppress-hover' on toggle click if we are hovering the sidebar area.

      if (toggleBtn) {
        const wrapper = document.getElementById('lpx-wrapper');
        const sidebar = document.querySelector('.lpx-sidebar-container');

        if (wrapper && sidebar) {
          // Add suppression class
          wrapper.classList.add('suppress-hover');

          // Remove it once mouse leaves the sidebar
          // effectively re-enabling hover expansion
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
    // Check if user is authenticated either via the passed user object or the service
    const isAuthenticated = user?.id || this.authService.isAuthenticated;
    const isLoginPage = this.router.url === '/' && !isAuthenticated;

    if (isLoginPage) {
      document.body.classList.add('hide-navigation');
    } else {
      document.body.classList.remove('hide-navigation');
    }
  }

  private configureOAuth() {
    console.log('Configuring OAuth...');
    if (typeof window !== 'undefined') {
      console.log('Setting localStorage for OAuth...');
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

    this.oauthService.loadDiscoveryDocument().then(doc => {
      console.log('App Init: Discovery Document Loaded', doc);
      console.log('App Init: Has Valid Token?', this.oauthService.hasValidAccessToken());
      console.log('App Init: Stored Token', this.oauthService.getAccessToken());
    });
  }
}
