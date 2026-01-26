using MongoDB.Driver;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace UCCP.SBD.Membership.MongoDB;

public class MongoDbConnectionStringConfigurator : ITransientDependency
{
    public void Configure(AbpMongoDbContextOptions options)
    {
        // Configure MongoDB client settings to bypass SSL certificate validation
        // This is needed for .NET 9 in Docker containers connecting to MongoDB Atlas
        options.ConfigureMongoClient = (connectionString, settings) =>
        {
            settings.SslSettings = new SslSettings
            {
                ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            return settings;
        };
    }
}
