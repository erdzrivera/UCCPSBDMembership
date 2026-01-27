import { Environment } from '@abp/ng.core';

const baseUrl = 'https://erdzrivera.github.io/UCCPSBDMembership';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'UCCP SBD Membership',
    logoUrl: '/UCCPSBDMembership/assets/images/logo/uccp-text-logo.png',
  },
  oAuthConfig: {
    issuer: 'https://uccp-membership-auth.onrender.com/',
    redirectUri: baseUrl,
    clientId: 'Membership_App',
    responseType: 'code',
    scope: 'openid offline_access Membership',
    requireHttps: true,
    oidc: false
  },
  apis: {
    default: {
      url: 'https://uccp-membership-api.onrender.com',
      rootNamespace: 'UCCP.SBD.Membership',
    },
  },
} as Environment;


