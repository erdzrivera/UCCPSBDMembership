# Redis Cloud Setup Guide

This guide walks you through setting up a free Redis Cloud database for caching in the UCCP SBD Membership application.

## Step 1: Create Redis Cloud Account

1. Go to [https://redis.com/try-free/](https://redis.com/try-free/)
2. Click **"Get Started Free"**
3. Sign up with your email or Google account
4. Verify your email address

## Step 2: Create a Free Database

1. After logging in, you'll see the **"Create database"** screen
2. Select **"Free"** plan
   - 30 MB storage
   - No credit card required
   - Shared resources
3. Click **"Let's start free"**

## Step 3: Configure Database

1. **Cloud Provider**: Choose AWS, Google Cloud, or Azure
2. **Region**: Select closest to your backend deployment
   - If using Azure App Service in Southeast Asia, choose Singapore
3. **Database Name**: `uccp-membership-cache`
4. Click **"Activate"**

> [!NOTE]
> Database activation takes 1-2 minutes.

## Step 4: Get Connection Details

1. Once activated, click on your database name
2. You'll see the **"Configuration"** tab with:
   - **Public endpoint**: `redis-xxxxx.c123.us-east-1-2.ec2.cloud.redislabs.com:12345`
   - **Default user password**: Click **"Show"** to reveal

## Step 5: Get Connection String

The connection string format for .NET is:
```
<endpoint>:<port>,password=<password>,ssl=True,abortConnect=False
```

Example:
```
redis-12345.c123.us-east-1-2.ec2.cloud.redislabs.com:12345,password=YOUR_PASSWORD,ssl=True,abortConnect=False
```

### Copy Your Connection String

1. Note your endpoint (e.g., `redis-12345.c123.us-east-1-2.ec2.cloud.redislabs.com:12345`)
2. Copy your password
3. Combine them:
   ```
   redis-12345.c123.us-east-1-2.ec2.cloud.redislabs.com:12345,password=YOUR_PASSWORD_HERE,ssl=True,abortConnect=False
   ```

## Step 6: Test Connection Locally

1. Open `aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host/appsettings.Development.json`
2. Update the Redis configuration:
   ```json
   {
     "Redis": {
       "Configuration": "redis-12345.c123.us-east-1-2.ec2.cloud.redislabs.com:12345,password=YOUR_PASSWORD,ssl=True,abortConnect=False"
     }
   }
   ```
3. Run the API locally to test:
   ```bash
   cd aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host
   dotnet run
   ```

## Step 7: Configure for Azure App Service

You'll add this connection string to Azure App Service configuration.

**Environment Variable Name**: `Redis__Configuration`
**Value**: Your Redis Cloud connection string

---

## Connection String Format

```
<host>:<port>,password=<password>,ssl=True,abortConnect=False
```

- **host**: Your Redis endpoint hostname
- **port**: Your Redis port (usually 5 digits)
- **password**: The password from Redis Cloud dashboard
- **ssl=True**: Required for Redis Cloud
- **abortConnect=False**: Prevents connection failures from crashing the app

---

## Monitoring and Management

### View Database Info
1. In Redis Cloud dashboard, click your database
2. **Metrics** tab shows:
   - Memory usage
   - Operations per second
   - Connected clients

### Test Connection
1. Click **"Connect"** button in dashboard
2. Use **Redis CLI** option to test commands

### View Keys (Optional)
1. Install Redis Insight: [https://redis.com/redis-enterprise/redis-insight/](https://redis.com/redis-enterprise/redis-insight/)
2. Connect using your endpoint and password
3. Browse keys and values

---

## Upgrade When Needed

### Free Tier Limits
- 30 MB storage
- Shared resources
- Good for development and testing

### Paid Tiers
- **250 MB**: $5/month
- **1 GB**: $12/month
- Dedicated resources
- Better performance

---

## Troubleshooting

### Connection Timeout
- Verify endpoint and port are correct
- Check firewall settings (Redis Cloud allows all IPs by default)
- Ensure `ssl=True` is in connection string

### Authentication Failed
- Double-check password
- Password is case-sensitive
- No spaces in connection string

### "No connection available"
- Add `abortConnect=False` to connection string
- This allows the app to start even if Redis is temporarily unavailable

---

## Alternative: Azure Cache for Redis

If you prefer to keep everything in Azure:

1. Azure Cache for Redis has a **Basic C0** tier at ~$15/month (250 MB)
2. No free tier available
3. Better integration with Azure App Service
4. Connection string format is similar

For now, Redis Cloud free tier is recommended to keep costs at $0.
