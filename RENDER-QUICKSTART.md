# 🚀 Render.com Deployment - Quick Start

## ✅ What's Ready

- [x] `render.yaml` configuration file created
- [x] Angular environment updated with Render.com URLs
- [x] Production config files updated
- [x] MongoDB Atlas connected
- [x] Redis Cloud connected

## 📝 Quick Steps (15 minutes total)

### 1. Sign Up for Render.com (2 min)
1. Go to: https://render.com/
2. Click **"Get Started for Free"**
3. Sign up with **GitHub** (easiest!)
4. **NO CREDIT CARD REQUIRED** ✅

### 2. Push Your Code (1 min)
```bash
git add .
git commit -m "Add Render.com deployment configuration"
git push origin main
```

### 3. Create Services on Render (5 min)
1. In Render Dashboard, click **"New"** → **"Blueprint"**
2. Select repository: `erdzrivera/UCCPSBDMembership`
3. Render will detect `render.yaml` automatically
4. Click **"Apply"**

### 4. Configure Environment Variables (5 min)

Render will create 2 services. For **EACH service**, add these variables:

#### For `uccp-membership-api`:
```
ConnectionStrings__Default = mongodb+srv://uccp_sbd_user:SNF2khiPRascE9Nj@uccpsbdmembership.zuadzdl.mongodb.net/Membership?appName=UCCPSBDMembership

Redis__Configuration = YOUR_REDIS_CLOUD_CONNECTION_STRING

AuthServer__Authority = https://uccp-membership-auth.onrender.com

OpenIddict__Applications__Membership_Swagger__RootUrl = https://uccp-membership-api.onrender.com
```

#### For `uccp-membership-auth`:
```
ConnectionStrings__Default = mongodb+srv://uccp_sbd_user:SNF2khiPRascE9Nj@uccpsbdmembership.zuadzdl.mongodb.net/Membership?appName=UCCPSBDMembership

Redis__Configuration = YOUR_REDIS_CLOUD_CONNECTION_STRING

App__SelfUrl = https://uccp-membership-auth.onrender.com

AuthServer__Authority = https://uccp-membership-auth.onrender.com

OpenIddict__Applications__Membership_Swagger__RootUrl = https://uccp-membership-auth.onrender.com
```

**Replace `YOUR_REDIS_CLOUD_CONNECTION_STRING`** with your actual Redis connection string!

### 5. Wait for Deployment (5-10 min)
- Render will automatically build and deploy
- Watch the logs in real-time
- Wait for both services to show "Live" status

### 6. Enable GitHub Pages (2 min)
1. Go to: https://github.com/erdzrivera/UCCPSBDMembership/settings/pages
2. Under "Build and deployment" → Source: **GitHub Actions**
3. Save

### 7. Deploy Frontend (automatic)
- GitHub Actions will automatically deploy
- Go to: https://github.com/erdzrivera/UCCPSBDMembership/actions
- Wait for workflow to complete (~3-5 min)

## 🎉 You're Live!

Your app will be accessible at:
- **Frontend**: https://erdzrivera.github.io/UCCPSBDMembership
- **API**: https://uccp-membership-api.onrender.com
- **Auth**: https://uccp-membership-auth.onrender.com

**Total Cost: $0/month** 🎊

---

## 📚 Full Guide

For detailed instructions, see: [render-deployment-guide.md](render-deployment-guide.md)

---

## ⚠️ Important Note

**Free tier services sleep after 15 minutes of inactivity.**
- First request after sleep takes ~30 seconds
- Subsequent requests are fast
- This is normal for free tier!

---

## 🆘 Need Help?

Check the troubleshooting section in the full guide or review the logs in Render Dashboard.
