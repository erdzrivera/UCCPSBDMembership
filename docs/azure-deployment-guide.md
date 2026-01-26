# Azure App Service Deployment Guide

This guide walks you through deploying the UCCP SBD Membership backend services to Azure App Service Free Tier.

## Prerequisites

- Azure account (create free at [https://azure.microsoft.com/free/](https://azure.microsoft.com/free/))
- MongoDB Atlas connection string (from [mongodb-atlas-setup.md](mongodb-atlas-setup.md))
- Redis Cloud connection string (from [redis-cloud-setup.md](redis-cloud-setup.md))

---

## Part 1: Create Azure Resources

### Step 1: Create Resource Group

1. Go to [Azure Portal](https://portal.azure.com)
2. Search for **"Resource groups"** and click **"Create"**
3. Fill in:
   - **Subscription**: Your subscription
   - **Resource group**: `rg-uccp-membership`
   - **Region**: Southeast Asia (or closest to you)
4. Click **"Review + create"** → **"Create"**

### Step 2: Create App Service Plan

1. Search for **"App Service plans"** and click **"Create"**
2. Fill in:
   - **Resource Group**: `rg-uccp-membership`
   - **Name**: `plan-uccp-membership`
   - **Operating System**: Linux
   - **Region**: Same as resource group
   - **Pricing tier**: Click **"Explore pricing plans"**
     - Select **"Dev/Test"** tab
     - Choose **"F1 (Free)"** - 60 min/day compute, 1 GB RAM
3. Click **"Review + create"** → **"Create"**

### Step 3: Create Web App for API Host

1. Search for **"App Services"** and click **"Create"** → **"Web App"**
2. Fill in **Basics**:
   - **Resource Group**: `rg-uccp-membership`
   - **Name**: `uccp-membership-api` (must be globally unique, try adding numbers if taken)
   - **Publish**: Code
   - **Runtime stack**: .NET 9 (STS)
   - **Operating System**: Linux
   - **Region**: Same as resource group
   - **App Service Plan**: `plan-uccp-membership`
3. Click **"Review + create"** → **"Create"**
4. **Save the URL**: `https://uccp-membership-api.azurewebsites.net`

### Step 4: Create Web App for Auth Server

1. Repeat Step 3 with these changes:
   - **Name**: `uccp-membership-auth`
   - **Runtime stack**: .NET 9 (STS)
   - **App Service Plan**: `plan-uccp-membership` (same plan)
2. Click **"Review + create"** → **"Create"**
3. **Save the URL**: `https://uccp-membership-auth.azurewebsites.net`

---

## Part 2: Configure App Settings

### Configure API Host

1. Go to your **uccp-membership-api** App Service
2. In left menu, click **"Configuration"**
3. Under **"Application settings"**, click **"New application setting"** for each:

| Name | Value |
|------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__Default` | Your MongoDB Atlas connection string |
| `Redis__Configuration` | Your Redis Cloud connection string |
| `AuthServer__Authority` | `https://uccp-membership-auth.azurewebsites.net` |
| `App__CorsOrigins` | `https://erdzrivera.github.io` |
| `OpenIddict__Applications__Membership_App__RootUrl` | `https://erdzrivera.github.io/UCCPSBDMembership` |
| `OpenIddict__Applications__Membership_Swagger__RootUrl` | `https://uccp-membership-api.azurewebsites.net` |

4. Click **"Save"** at the top

### Configure Auth Server

1. Go to your **uccp-membership-auth** App Service
2. In left menu, click **"Configuration"**
3. Under **"Application settings"**, click **"New application setting"** for each:

| Name | Value |
|------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__Default` | Your MongoDB Atlas connection string |
| `Redis__Configuration` | Your Redis Cloud connection string |
| `App__SelfUrl` | `https://uccp-membership-auth.azurewebsites.net` |
| `App__ClientUrl` | `https://erdzrivera.github.io/UCCPSBDMembership` |
| `App__CorsOrigins` | `https://erdzrivera.github.io` |
| `App__RedirectAllowedUrls` | `https://erdzrivera.github.io/UCCPSBDMembership` |
| `AuthServer__Authority` | `https://uccp-membership-auth.azurewebsites.net` |
| `OpenIddict__Applications__Membership_App__RootUrl` | `https://erdzrivera.github.io/UCCPSBDMembership` |
| `OpenIddict__Applications__Membership_Swagger__RootUrl` | `https://uccp-membership-auth.azurewebsites.net` |

4. Click **"Save"** at the top

---

## Part 3: Set Up GitHub Actions Deployment

### Get Publish Profiles

#### For API Host:
1. Go to **uccp-membership-api** App Service
2. Click **"Download publish profile"** at the top
3. Open the downloaded file and copy ALL contents

#### For Auth Server:
1. Go to **uccp-membership-auth** App Service
2. Click **"Download publish profile"** at the top
3. Open the downloaded file and copy ALL contents

### Add GitHub Secrets

1. Go to your GitHub repository: `https://github.com/erdzrivera/UCCPSBDMembership`
2. Click **"Settings"** → **"Secrets and variables"** → **"Actions"**
3. Click **"New repository secret"** for each:

| Secret Name | Value |
|-------------|-------|
| `AZURE_API_APP_NAME` | `uccp-membership-api` |
| `AZURE_API_PUBLISH_PROFILE` | Paste the API publish profile contents |
| `AZURE_AUTH_APP_NAME` | `uccp-membership-auth` |
| `AZURE_AUTH_PUBLISH_PROFILE` | Paste the Auth publish profile contents |

---

## Part 4: Update Frontend Configuration

1. Open `angular/src/environments/environment.prod.ts`
2. Update with your Azure URLs:

```typescript
import { Environment } from '@abp/ng.core';

const baseUrl = 'https://erdzrivera.github.io/UCCPSBDMembership';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Membership',
    logoUrl: '/UCCPSBDMembership/assets/images/logo/uccp-text-logo.png',
  },
  oAuthConfig: {
    issuer: 'https://uccp-membership-auth.azurewebsites.net/',
    redirectUri: baseUrl,
    clientId: 'Membership_App',
    responseType: 'code',
    scope: 'openid offline_access Membership',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://uccp-membership-api.azurewebsites.net',
      rootNamespace: 'UCCP.SBD.Membership',
    },
  },
} as Environment;
```

---

## Part 5: Deploy

### Enable GitHub Pages

1. Go to your GitHub repository settings
2. Click **"Pages"** in left sidebar
3. Under **"Build and deployment"**:
   - **Source**: GitHub Actions
4. Click **"Save"**

### Trigger Deployment

1. Commit and push all changes:
   ```bash
   git add .
   git commit -m "Add deployment configuration"
   git push origin main
   ```

2. Go to **"Actions"** tab in GitHub
3. You should see workflows running:
   - **Deploy Frontend to GitHub Pages**
   - **Deploy Backend to Azure**

4. Wait for all workflows to complete (green checkmarks)

---

## Part 6: Initialize Database

### Run Database Migrator

1. Update `aspnet-core/src/UCCP.SBD.Membership.DbMigrator/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Default": "YOUR_MONGODB_ATLAS_CONNECTION_STRING"
     },
     "Redis": {
       "Configuration": "YOUR_REDIS_CLOUD_CONNECTION_STRING"
     },
     "AuthServer": {
       "Authority": "https://uccp-membership-auth.azurewebsites.net",
       "RequireHttpsMetadata": "true"
     }
   }
   ```

2. Run the migrator locally:
   ```bash
   cd aspnet-core/src/UCCP.SBD.Membership.DbMigrator
   dotnet run
   ```

3. This will seed initial data and create admin user

---

## Part 7: Test Your Deployment

1. **Frontend**: Visit `https://erdzrivera.github.io/UCCPSBDMembership`
2. **API Health**: Visit `https://uccp-membership-api.azurewebsites.net/health`
3. **Auth Server**: Visit `https://uccp-membership-auth.azurewebsites.net/.well-known/openid-configuration`
4. **Login**: Try logging in with default admin credentials

---

## Monitoring and Logs

### View Application Logs

1. Go to App Service in Azure Portal
2. Click **"Log stream"** in left menu
3. See real-time logs

### View Metrics

1. Click **"Metrics"** in left menu
2. View CPU, memory, HTTP requests

---

## Troubleshooting

### App won't start
- Check **"Log stream"** for errors
- Verify all configuration settings are correct
- Ensure MongoDB and Redis connection strings are valid

### CORS errors
- Verify `App__CorsOrigins` includes `https://erdzrivera.github.io`
- Check browser console for specific CORS error

### Authentication not working
- Verify Auth Server URL is correct in all configs
- Check `AuthServer__Authority` matches actual URL
- Ensure redirect URLs are configured correctly

### Free tier limitations
- App sleeps after 20 minutes of inactivity
- First request after sleep takes 10-20 seconds
- 60 minutes of compute per day limit
- Upgrade to Basic tier ($13/month) for always-on

---

## Upgrade to Paid Tier (Optional)

When ready for production:

1. Go to App Service Plan
2. Click **"Scale up (App Service plan)"**
3. Choose **"Production"** tab
4. Select **"B1 Basic"** ($13/month):
   - Always on
   - 1.75 GB RAM
   - Custom domains
   - SSL certificates

---

## Next Steps

- Set up custom domain (optional)
- Configure SSL certificate (free with App Service)
- Set up Application Insights for monitoring
- Configure auto-scaling (paid tiers only)
