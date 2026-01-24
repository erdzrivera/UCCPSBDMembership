using UCCP.SBD.Membership.Samples;
using Xunit;

namespace UCCP.SBD.Membership.MongoDB.Domains;

[Collection(MembershipTestConsts.CollectionDefinitionName)]
public class MongoDBSampleDomainTests : SampleDomainTests<MembershipMongoDbTestModule>
{

}
