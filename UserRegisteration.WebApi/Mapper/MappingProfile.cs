using AutoMapper;
using UserRegistration.Application.Services.IdentityService.DTOs;
using UserRegistration.Application.Services.LoanApplicationService.DTOs;
using UserRegistration.Common.ApplicationExtension;
using UserRegistration.Domain;

namespace UserRegisteration.WebApi.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            //Identity
            CreateMap<RegisterRequest, UserEntity>();


            //LoanApplication


            CreateMap<LoanApplicationEntity, LoanApplicationDTO>()
               .ForMember(x => x.LoanTypeString, o => o.MapFrom(s => UserRegistrationExtensions.GetEnumDescription(s.LoanType)))
               .ForMember(x => x.StatusTypeString, o => o.MapFrom(s => UserRegistrationExtensions.GetEnumDescription(s.Status)));
            CreateMap<CreateLoanApplicationRequest, LoanApplicationEntity>();
            CreateMap<UpdateLoanApplicationRequest, LoanApplicationEntity>();



            //CreateMap<Venue, VenueDTO>()
            //   .ForMember(x => x.TypeString, o => o.MapFrom(s => NanoHelpers.GetEnumDescription(s.Type)));
            //CreateMap<CreateVenueRequest, Venue>();
            //CreateMap<UpdateVenueRequest, Venue>();


        }
    }
}
