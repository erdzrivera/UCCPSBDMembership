using AutoMapper;
using UCCP.SBD.Membership.Members;

namespace UCCP.SBD.Membership;

public class MembershipApplicationAutoMapperProfile : Profile
{
    public MembershipApplicationAutoMapperProfile()
    {
        CreateMap<Member, MemberDto>();
        CreateMap<CreateUpdateMemberDto, Member>();
        
        CreateMap<MembershipType, MembershipTypeDto>().ReverseMap();
        CreateMap<Organization, OrganizationDto>().ReverseMap();
    }
}
