# GitHub Pages + Azure Deployment Guide

Complete guide for deploying the UCCP SBD Membership application using GitHub Pages (frontend) and Azure App Service (backend) with free-tier cloud databases.

## 📋 Overview

**Architecture:**
- **Frontend**: GitHub Pages (Free)
- **Backend API**: Azure App Service Free Tier
- **Auth Server**: Azure App Service Free Tier
- **Database**: MongoDB Atlas M0 (Free - 512MB)
- **Cache**: Redis Cloud (Free - 30MB)

**Total Cost**: $0/month 🎉

---

## 🚀 Quick Start Checklist

Follow these guides in order:

1. ✅ [MongoDB Atlas Setup](mongodb-atlas-setup.md) - 15 minutes
2. ✅ [Redis Cloud Setup](redis-cloud-setup.md) - 10 minutes
3. ✅ [Azure App Service Deployment](azure-deployment-guide.md) - 30 minutes
4. ✅ [GitHub Pages Setup](#github-pages-setup) - 10 minutes

**Total Time**: ~1-2 hours

---

## Prerequisites

- [x] GitHub account with repository: `https://github.com/erdzrivera/UCCPSBDMembership`
- [ ] Azure account (free tier available)
- [ ] MongoDB Atlas account (free tier)
- [ ] Redis Cloud account (free tier)

---

## Step-by-Step Deployment

### Phase 1: Database Setup (Day 1)

#### 1. MongoDB Atlas
Follow the [MongoDB Atlas Setup Guide](mongodb-atlas-setup.md) to:
- Create free M0 cluster
- Configure network access
- Create database user
- Get connection string

**Save**: MongoDB connection string for later

#### 2. Redis Cloud
Follow the [Redis Cloud Setup Guide](redis-cloud-setup.md) to:
- Create free 30MB database
- Get endpoint and password
- Format connection string

**Save**: Redis connection string for later

---

### Phase 2: Backend Deployment (Day 2)

Follow the [Azure Deployment Guide](azure-deployment-guide.md) to:
- Create Azure App Service resources
- Configure environment variables
- Set up GitHub Actions secrets
- Deploy backend services

**URLs you'll get**:
- API: `https://uccp-membership-api.azurewebsites.net`
- Auth: `https://uccp-membership-auth.azurewebsites.net`

---

### Phase 3: Frontend Deployment (Day 3)

#### GitHub Pages Setup

1. **Enable GitHub Pages**
   - Go to repository settings: `https://github.com/erdzrivera/UCCPSBDMembership/settings/pages`
   - Under **"Build and deployment"**:
     - **Source**: GitHub Actions
   - Click **"Save"**

2. **Verify Configuration**
   - Check `angular/src/environments/environment.prod.ts` has correct URLs:
     ```typescript
     issuer: 'https://uccp-membership-auth.azurewebsites.net/'
     url: 'https://uccp-membership-api.azurewebsites.net'
     ```

3. **Deploy**
   ```bash
   git add .
   git commit -m "Configure production deployment"
   git push origin main
   ```

4. **Monitor Deployment**
   - Go to **Actions** tab in GitHub
   - Watch **"Deploy Frontend to GitHub Pages"** workflow
   - Wait for green checkmark (3-5 minutes)

5. **Access Application**
   - Frontend: `https://erdzrivera.github.io/UCCPSBDMembership`
   - May take 1-2 minutes after deployment to be accessible

---

### Phase 4: Initialize Database (Day 3)

1. **Update DbMigrator Configuration**
   
   Edit `aspnet-core/src/UCCP.SBD.Membership.DbMigrator/appsettings.json`:
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
     },
     "OpenIddict": {
       "Applications": {
         "Membership_App": {
           "ClientId": "Membership_App",
           "RootUrl": "https://erdzrivera.github.io/UCCPSBDMembership"
         }
       }
     }
   }
   ```

2. **Run Database Migrator**
   ```bash
   cd aspnet-core/src/UCCP.SBD.Membership.DbMigrator
   dotnet run
   ```

3. **Verify**
   - Check MongoDB Atlas dashboard for `Membership` database
   - Should see collections created

---

## 🧪 Testing Your Deployment

### 1. Health Checks

```bash
# API Health
curl https://uccp-membership-api.azurewebsites.net/health

# Auth Server OpenID Configuration
curl https://uccp-membership-auth.azurewebsites.net/.well-known/openid-configuration
```

### 2. Frontend Access

1. Visit: `https://erdzrivera.github.io/UCCPSBDMembership`
2. Should see login page
3. Try logging in with default admin credentials (from DbMigrator output)

### 3. Full User Flow

- [ ] User registration
- [ ] User login
- [ ] View organizations
- [ ] Create organization
- [ ] View members
- [ ] Create member
- [ ] Logout

---

## 📊 Monitoring

### GitHub Actions

- **Frontend**: `https://github.com/erdzrivera/UCCPSBDMembership/actions`
- Automatic deployment on every push to `main`

### Azure App Service

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to your App Services
3. Click **"Log stream"** to see real-time logs
4. Click **"Metrics"** to see performance

### MongoDB Atlas

1. Go to [MongoDB Atlas](https://cloud.mongodb.com)
2. Click **"Metrics"** to see database usage
3. Click **"Browse Collections"** to view data

---

## 🔧 Troubleshooting

### Frontend Issues

**Problem**: 404 on GitHub Pages
- **Solution**: Wait 1-2 minutes after deployment, check Actions tab for errors

**Problem**: Blank page
- **Solution**: Check browser console, verify API URLs in environment.prod.ts

### Backend Issues

**Problem**: App not responding
- **Solution**: Free tier sleeps after inactivity, first request takes 10-20 seconds

**Problem**: 500 errors
- **Solution**: Check Azure Log Stream, verify MongoDB and Redis connections

### Authentication Issues

**Problem**: Login redirects fail
- **Solution**: Verify all URLs in Auth Server configuration match exactly

**Problem**: CORS errors
- **Solution**: Check `App__CorsOrigins` in Azure configuration includes GitHub Pages URL

---

## 📈 Upgrade Path

### When to Upgrade

Upgrade when you experience:
- App sleeping too often (free tier sleeps after 20 min inactivity)
- Hitting 60 min/day compute limit
- Need more than 512MB database storage
- Need better performance

### Upgrade Costs

**Minimal Production Setup** (~$76/month):
- Azure App Service Basic B1: $13/month × 2 = $26/month
- MongoDB Atlas M10: $57/month
- Redis Cloud 250MB: $5/month

**Benefits**:
- Always-on (no sleeping)
- Better performance
- More storage
- Custom domains
- SSL certificates

---

## 🔐 Security Considerations

### Before Going to Production

1. **Change Encryption Key**
   - Update `StringEncryption.DefaultPassPhrase` in production configs
   - Use Azure Key Vault for secrets

2. **Restrict MongoDB Access**
   - Change from "Allow from anywhere" to specific IPs
   - Use Azure App Service outbound IPs

3. **Enable HTTPS Only**
   - Already configured in production settings
   - Verify `RequireHttpsMetadata: true`

4. **Review CORS**
   - Ensure only your domain is allowed
   - Remove wildcard origins

---

## 📚 Additional Resources

- [MongoDB Atlas Documentation](https://docs.atlas.mongodb.com/)
- [Redis Cloud Documentation](https://docs.redis.com/latest/rc/)
- [Azure App Service Documentation](https://docs.microsoft.com/azure/app-service/)
- [GitHub Pages Documentation](https://docs.github.com/pages)

---

## 🆘 Getting Help

If you encounter issues:

1. Check the troubleshooting section above
2. Review Azure Log Stream for backend errors
3. Check GitHub Actions logs for deployment errors
4. Verify all connection strings are correct
5. Ensure all URLs match exactly (no trailing slashes where not needed)

---

## ✅ Deployment Checklist

Use this checklist to track your progress:

- [ ] MongoDB Atlas cluster created
- [ ] MongoDB connection string saved
- [ ] Redis Cloud database created
- [ ] Redis connection string saved
- [ ] Azure Resource Group created
- [ ] Azure App Service Plan created
- [ ] API Host App Service created
- [ ] Auth Server App Service created
- [ ] API Host configured with environment variables
- [ ] Auth Server configured with environment variables
- [ ] GitHub secrets added (publish profiles)
- [ ] Frontend environment.prod.ts updated
- [ ] GitHub Pages enabled
- [ ] Code pushed to GitHub
- [ ] GitHub Actions workflows completed successfully
- [ ] Database migrator run
- [ ] Frontend accessible at GitHub Pages URL
- [ ] API health check passes
- [ ] Auth server OpenID config accessible
- [ ] User can log in successfully
- [ ] All CRUD operations work

---

**Congratulations!** 🎉 Your application is now deployed and accessible worldwide for free!
