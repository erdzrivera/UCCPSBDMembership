using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace UCCP.SBD.Membership.Members
{
    public class MembershipDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<MembershipType, string> _membershipTypeRepository;
        private readonly IRepository<Organization, string> _organizationRepository;

        public MembershipDataSeederContributor(
            IRepository<MembershipType, string> membershipTypeRepository,
            IRepository<Organization, string> organizationRepository)
        {
            _membershipTypeRepository = membershipTypeRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _membershipTypeRepository.GetCountAsync() <= 0)
            {
                await _membershipTypeRepository.InsertAsync(new MembershipType("1", "Regular", "Regular Member"));
                await _membershipTypeRepository.InsertAsync(new MembershipType("2", "Associate", "Associate Member"));
                await _membershipTypeRepository.InsertAsync(new MembershipType("3", "Affiliate", "Affiliate Member"));
                await _membershipTypeRepository.InsertAsync(new MembershipType("4", "Preparatory", "Preparatory Member"));
                await _membershipTypeRepository.InsertAsync(new MembershipType("5", "Honorary", "Honorary Member"));
            }

            if (await _organizationRepository.GetCountAsync() <= 0)
            {
                await _organizationRepository.InsertAsync(new Organization("1", "United Church Senior Citizen's Association", "UCSCA"));
                await _organizationRepository.InsertAsync(new Organization("2", "United Church Men", "UCM"));
                await _organizationRepository.InsertAsync(new Organization("3", "Christian Women's Association", "CWA"));
                await _organizationRepository.InsertAsync(new Organization("4", "Christian Youth Adult Fellowship","CYAF"));
                await _organizationRepository.InsertAsync(new Organization("5", "Christian Youth Fellowship", "CYF"));
                await _organizationRepository.InsertAsync(new Organization("6", "Kids", "KIDS"));
            }
        }
    }
}
