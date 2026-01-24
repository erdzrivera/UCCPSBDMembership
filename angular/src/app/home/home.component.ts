import { AuthService, AbpApplicationConfigurationService, ConfigStateService } from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  private authService = inject(AuthService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);
  private abpConfigService = inject(AbpApplicationConfigurationService);
  private configState = inject(ConfigStateService);

  username = '';
  password = '';
  isLoggingIn = false;
  loginError = '';

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.loginError = '';
    this.isLoggingIn = true;

    // Load discovery document first to get the token endpoint
    console.log('Login started for:', this.username);
    this.oauthService.loadDiscoveryDocument()
      .then((doc) => {
        console.log('Discovery document loaded', doc);
        return this.oauthService.fetchTokenUsingPasswordFlow(this.username, this.password);
      })
      .then((tokenResponse) => {
        console.log('Token received', tokenResponse);
        return this.oauthService.loadUserProfile();
      })
      .then((userProfile) => {
        console.log('User profile loaded', userProfile);

        // Force valid token check
        const hasToken = this.oauthService.hasValidAccessToken();
        console.log('Has valid access token?', hasToken);
        console.log('Current token:', this.oauthService.getAccessToken());

        if (hasToken) {
          console.log('Refreshing app config...');
          return this.abpConfigService.get({ includeLocalizationResources: false }).toPromise();
        }
        console.warn('No valid token found despite successful fetch!');
        return Promise.resolve(null);
      })
      .then((config) => {
        console.log('App config refreshed', config);

        if (config) {
          // MANUALLY UPDATE THE GLOBAL STATE to refresh the UI (Header, Menu, etc.)
          this.configState.setState(config);
        }

        console.log('Current User State:', config?.currentUser);
        console.log('Granted Policies:', config?.auth?.grantedPolicies);
        this.isLoggingIn = false;
        this.isLoggingIn = false;
        // Auto-navigate commented out to show the Home Page content first
        // this.router.navigate(['/members']);
      })
      .catch((err) => {
        this.isLoggingIn = false;
        console.error('Login error detailed:', err);
        if (err.error === 'invalid_grant' || (err.status === 400 && err.error?.error === 'invalid_grant')) {
          this.loginError = 'Invalid username or password';
        } else {
          this.loginError = 'An error occurred during login. Check console.';
        }
      });
  }
}
