import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'UCCP SBD Membership',
    logoUrl: 'assets/images/logo/uccp-text-logo.png',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44361/',
    redirectUri: baseUrl,
    clientId: 'Membership_App',
    responseType: 'code',
    scope: 'openid offline_access Membership',
    requireHttps: false,
    oidc: false,
  },
  apis: {
    default: {
      url: 'https://localhost:44343',
      rootNamespace: 'UCCP.SBD.Membership',
    },
  },
} as Environment;
