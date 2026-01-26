# Render.com Deployment Guide

Complete guide for deploying the UCCP SBD Membership application backend to Render.com (FREE tier).

## 🎯 Why Render.com?

- ✅ **No credit card required** for free tier
- ✅ **No quota restrictions** like Azure
- ✅ **Automatic deployments** from GitHub
- ✅ **Free SSL/HTTPS** included
- ✅ **Simple setup** - one YAML file

---

## 📋 Prerequisites

- [x] GitHub repository: `https://github.com/erdzrivera/UCCPSBDMembership`
- [x] MongoDB Atlas connection string
- [x] Redis Cloud connection string
- [ ] Render.com account (free - no card needed)

---

## 🚀 Step-by-Step Deployment

### Step 1: Create Render.com Account (2 minutes)

1. Go to [https://render.com/](https://render.com/)
2. Click **"Get Started for Free"**
3. Sign up with **GitHub** (recommended for easy integration)
4. Authorize Render to access your GitHub repositories

---

### Step 2: Connect Your Repository (1 minute)

1. After signing in, you'll see the Render Dashboard
2. Render will automatically detect your `render.yaml` file
3. Click **"New"** → **"Blueprint"**
4. Select your repository: `erdzrivera/UCCPSBDMembership`
5. Render will read the `render.yaml` configuration

---

### Step 3: Configure Environment Variables (5 minutes)

Render will create 2 services based on `render.yaml`:
- `uccp-membership-api`
- `uccp-membership-auth`

For **EACH service**, you need to set these environment variables:

#### For Both Services (API + Auth):

| Environment Variable | Value |
|---------------------|-------|
| `ConnectionStrings__Default` | Your MongoDB Atlas connection string |
| `Redis__Configuration` | Your Redis Cloud connection string |

#### For API Service Only:

| Environment Variable | Value |
|---------------------|-------|
| `AuthServer__Authority` | `https://uccp-membership-auth.onrender.com` |
| `OpenIddict__Applications__Membership_Swagger__RootUrl` | `https://uccp-membership-api.onrender.com` |

#### For Auth Service Only:

| Environment Variable | Value |
|---------------------|-------|
| `App__SelfUrl` | `https://uccp-membership-auth.onrender.com` |
| `AuthServer__Authority` | `https://uccp-membership-auth.onrender.com` |
| `OpenIddict__Applications__Membership_Swagger__RootUrl` | `https://uccp-membership-auth.onrender.com` |

**How to add environment variables:**
1. Click on a service name
2. Go to **"Environment"** tab
3. Click **"Add Environment Variable"**
4. Enter key and value
5. Click **"Save Changes"**

---

### Step 4: Deploy (Automatic!)

1. Once environment variables are set, Render will **automatically deploy**
2. Watch the build logs in real-time
3. First deployment takes ~5-10 minutes
4. You'll see:
   - ✅ Build successful
   - ✅ Service live

---

### Step 5: Get Your URLs

After deployment, you'll have:
- **API**: `https://uccp-membership-api.onrender.com`
- **Auth**: `https://uccp-membership-auth.onrender.com`

**Save these URLs** - you'll need them for the frontend!

---

### Step 6: Update Frontend Configuration (2 minutes)

Update `angular/src/environments/environment.prod.ts`:

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
    issuer: 'https://uccp-membership-auth.onrender.com/',
    redirectUri: baseUrl,
    clientId: 'Membership_App',
    responseType: 'code',
    scope: 'openid offline_access Membership',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://uccp-membership-api.onrender.com',
      rootNamespace: 'UCCP.SBD.Membership',
    },
  },
} as Environment;
```

---

### Step 7: Update Production Config Files

Update the Auth Server URL in production configs:

#### `aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host/appsettings.Production.json`:
```json
{
  "AuthServer": {
    "Authority": "https://uccp-membership-auth.onrender.com",
    "RequireHttpsMetadata": true
  }
}
```

#### `aspnet-core/src/UCCP.SBD.Membership.AuthServer/appsettings.Production.json`:
```json
{
  "App": {
    "SelfUrl": "https://uccp-membership-auth.onrender.com"
  },
  "AuthServer": {
    "Authority": "https://uccp-membership-auth.onrender.com"
  }
}
```

---

### Step 8: Deploy Frontend to GitHub Pages (5 minutes)

1. **Commit and push your changes:**
   ```bash
   git add .
   git commit -m "Configure for Render.com deployment"
   git push origin main
   ```

2. **Enable GitHub Pages:**
   - Go to: `https://github.com/erdzrivera/UCCPSBDMembership/settings/pages`
   - Under "Build and deployment" → Source: **GitHub Actions**
   - Save

3. **Wait for deployment:**
   - Go to: `https://github.com/erdzrivera/UCCPSBDMembership/actions`
   - Watch the workflow complete (~3-5 minutes)

---

## 🧪 Testing Your Deployment

### 1. Test Backend Services

```bash
# Test API Health
curl https://uccp-membership-api.onrender.com/health

# Test Auth Server
curl https://uccp-membership-auth.onrender.com/.well-known/openid-configuration
```

### 2. Test Frontend

1. Visit: `https://erdzrivera.github.io/UCCPSBDMembership`
2. Try logging in with admin credentials
3. Test all features

---

## ⚠️ Important Notes

### Free Tier Limitations

- **Services sleep after 15 minutes** of inactivity
- **First request after sleep** takes ~30 seconds to wake up
- **750 hours/month** of free compute per service
- **No credit card required**

### Wake-Up Time

When your app sleeps:
1. First visitor waits ~30 seconds
2. Subsequent requests are fast
3. App stays awake while in use

**Tip**: Use a service like [UptimeRobot](https://uptimerobot.com/) to ping your app every 14 minutes to keep it awake (optional).

---

## 📊 Monitoring

### View Logs

1. Go to Render Dashboard
2. Click on a service
3. Click **"Logs"** tab
4. See real-time logs

### View Metrics

1. Click **"Metrics"** tab
2. See CPU, memory, and request metrics

---

## 🔄 Automatic Deployments

Every time you push to `main` branch:
- Render **automatically rebuilds** and deploys
- No manual intervention needed
- See deployment status in Render Dashboard

---

## 💰 Cost Breakdown

| Service | Cost |
|---------|------|
| Render.com API Service | $0 |
| Render.com Auth Service | $0 |
| MongoDB Atlas M0 | $0 |
| Redis Cloud 30MB | $0 |
| GitHub Pages | $0 |
| **Total** | **$0/month** |

---

## 🆙 Upgrade Path

When you need always-on services:

**Render.com Starter Plan**: $7/month per service
- No sleep
- Always on
- Better performance
- 512 MB RAM

**Total for 2 services**: $14/month

---

## 🔧 Troubleshooting

### Service Won't Start

**Check logs:**
1. Go to service in Render Dashboard
2. Click "Logs"
3. Look for errors

**Common issues:**
- Missing environment variables
- Wrong MongoDB/Redis connection strings
- Build errors

### CORS Errors

**Verify:**
- `App__CorsOrigins` includes `https://erdzrivera.github.io`
- No trailing slashes in URLs
- HTTPS is used everywhere

### Authentication Not Working

**Check:**
- Auth Server URL is correct in all configs
- OpenIddict configuration matches
- Database was initialized with DbMigrator

---

## ✅ Deployment Checklist

- [ ] Render.com account created
- [ ] Repository connected to Render
- [ ] Both services created from Blueprint
- [ ] Environment variables configured for API service
- [ ] Environment variables configured for Auth service
- [ ] Services deployed successfully
- [ ] Service URLs saved
- [ ] Frontend environment.prod.ts updated
- [ ] Production config files updated
- [ ] Code pushed to GitHub
- [ ] GitHub Pages enabled
- [ ] Frontend deployed
- [ ] Backend health checks pass
- [ ] Frontend loads successfully
- [ ] Login works
- [ ] All features tested

---

## 🎉 Success!

Your app is now deployed and accessible at:
- **Frontend**: `https://erdzrivera.github.io/UCCPSBDMembership`
- **API**: `https://uccp-membership-api.onrender.com`
- **Auth**: `https://uccp-membership-auth.onrender.com`

**Total cost: $0/month** 🎊

---

## 📚 Additional Resources

- [Render.com Documentation](https://render.com/docs)
- [Render.com Free Tier Details](https://render.com/docs/free)
- [Render Blueprint Spec](https://render.com/docs/blueprint-spec)
