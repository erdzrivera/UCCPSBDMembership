using UCCP.SBD.Membership.MongoDB;
using UCCP.SBD.Membership.Samples;
using Xunit;

namespace UCCP.SBD.Membership.MongoDb.Applications;

[Collection(MembershipTestConsts.CollectionDefinitionName)]
public class MongoDBSampleAppServiceTests : SampleAppServiceTests<MembershipMongoDbTestModule>
{

}
