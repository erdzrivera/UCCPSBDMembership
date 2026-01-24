import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Membership',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44361/',
    redirectUri: baseUrl,
    clientId: 'Membership_App',
    responseType: 'code',
    scope: 'offline_access Membership',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44343',
      rootNamespace: 'UCCP.SBD.Membership',
    },
  },
} as Environment;
