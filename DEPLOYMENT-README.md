# 🚀 Deployment Quick Start

This repository is configured for automatic deployment to:
- **Frontend**: GitHub Pages (Free)
- **Backend**: Azure App Service (Free Tier)
- **Database**: MongoDB Atlas (Free Tier)
- **Cache**: Redis Cloud (Free Tier)

## 📖 Complete Deployment Guide

Follow the step-by-step guide: **[docs/DEPLOYMENT-GITHUB-PAGES.md](docs/DEPLOYMENT-GITHUB-PAGES.md)**

## ⚡ Quick Deploy (1-2 hours)

### 1. Setup Databases (30 min)
- [MongoDB Atlas Setup](docs/mongodb-atlas-setup.md)
- [Redis Cloud Setup](docs/redis-cloud-setup.md)

### 2. Deploy Backend (30 min)
- [Azure App Service Guide](docs/azure-deployment-guide.md)

### 3. Deploy Frontend (15 min)
- Enable GitHub Pages in repository settings
- Push code to trigger deployment

### 4. Initialize Database (15 min)
- Run DbMigrator with cloud connection strings

## 🌐 Your URLs

After deployment, your app will be available at:
- **Frontend**: `https://erdzrivera.github.io/UCCPSBDMembership`
- **API**: `https://uccp-membership-api.azurewebsites.net`
- **Auth**: `https://uccp-membership-auth.azurewebsites.net`

## 💰 Cost

**Total: $0/month** with free tiers

Upgrade path available when needed (~$76/month for production-ready setup)

## 📝 What's Included

- ✅ GitHub Actions workflows for CI/CD
- ✅ Production configuration files
- ✅ Comprehensive deployment guides
- ✅ MongoDB Atlas integration
- ✅ Redis Cloud integration
- ✅ Azure App Service configuration

## 🆘 Need Help?

See troubleshooting section in [DEPLOYMENT-GITHUB-PAGES.md](docs/DEPLOYMENT-GITHUB-PAGES.md)
