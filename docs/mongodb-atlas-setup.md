# MongoDB Atlas Setup Guide

This guide walks you through setting up a free MongoDB Atlas cluster for the UCCP SBD Membership application.

## Step 1: Create MongoDB Atlas Account

1. Go to [https://www.mongodb.com/cloud/atlas/register](https://www.mongodb.com/cloud/atlas/register)
2. Sign up with your email or Google account
3. Complete the registration process

## Step 2: Create a Free Cluster

1. After logging in, click **"Build a Database"**
2. Select **"M0 FREE"** tier
   - 512 MB storage
   - Shared RAM
   - No credit card required
3. Choose your cloud provider and region:
   - **Provider**: AWS, Google Cloud, or Azure
   - **Region**: Choose closest to your users (e.g., Singapore, Tokyo for Asia)
4. **Cluster Name**: `UCCPMembership` (or keep default)
5. Click **"Create Cluster"**

> [!NOTE]
> Cluster creation takes 3-5 minutes.

## Step 3: Configure Network Access

1. In the left sidebar, click **"Network Access"**
2. Click **"Add IP Address"**
3. For testing, click **"Allow Access from Anywhere"** (0.0.0.0/0)
   - ⚠️ For production, restrict to specific IPs
4. Click **"Confirm"**

## Step 4: Create Database User

1. In the left sidebar, click **"Database Access"**
2. Click **"Add New Database User"**
3. Choose **"Password"** authentication
4. Set username: `membership_user`
5. Click **"Autogenerate Secure Password"** and **SAVE THIS PASSWORD**
6. Under **"Database User Privileges"**, select **"Read and write to any database"**
7. Click **"Add User"**

## Step 5: Get Connection String

1. Go to **"Database"** in the left sidebar
2. Click **"Connect"** on your cluster
3. Select **"Connect your application"**
4. Choose:
   - **Driver**: C# / .NET
   - **Version**: 2.13 or later
5. Copy the connection string:
   ```
   mongodb+srv://membership_user:<password>@cluster0.xxxxx.mongodb.net/?retryWrites=true&w=majority
   ```
6. Replace `<password>` with the password you saved earlier
7. Add the database name: `/Membership` before the `?`
   ```
   mongodb+srv://membership_user:YOUR_PASSWORD@cluster0.xxxxx.mongodb.net/Membership?retryWrites=true&w=majority
   ```

## Step 6: Test Connection Locally

1. Open `aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host/appsettings.Development.json`
2. Update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "Default": "mongodb+srv://membership_user:YOUR_PASSWORD@cluster0.xxxxx.mongodb.net/Membership?retryWrites=true&w=majority"
     }
   }
   ```
3. Run the API locally to test:
   ```bash
   cd aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host
   dotnet run
   ```

## Step 7: Configure for Azure App Service

You'll add this connection string to Azure App Service configuration in the Azure setup guide.

**Environment Variable Name**: `ConnectionStrings__Default`
**Value**: Your MongoDB Atlas connection string

---

## Connection String Format

```
mongodb+srv://<username>:<password>@<cluster-url>/<database-name>?retryWrites=true&w=majority
```

- **username**: `membership_user`
- **password**: The password you saved
- **cluster-url**: From your Atlas cluster (e.g., `cluster0.xxxxx.mongodb.net`)
- **database-name**: `Membership`

---

## Monitoring and Management

### View Data
1. In Atlas dashboard, click **"Browse Collections"**
2. You can view, add, edit, and delete documents

### Monitor Usage
1. Click **"Metrics"** to see:
   - Storage usage
   - Connection count
   - Operations per second

### Upgrade When Needed
- Free tier: 512 MB storage
- When you need more, upgrade to M10 ($57/month) for 10 GB storage

---

## Troubleshooting

### Connection Timeout
- Check Network Access allows your IP
- Verify username and password are correct
- Ensure connection string includes database name

### Authentication Failed
- Double-check password (no special characters need escaping in connection string)
- Verify user has correct permissions

### Database Not Created
- Database is created automatically when first document is inserted
- Run the DbMigrator project to seed initial data
