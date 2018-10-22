using AutoMapper;
using ExodusKorea.Model.Entities;

namespace ExodusKorea.Model.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to ViewModel
            CreateMap<ApplicationUser, ApplicationUserVM>();

            // ViewModel to Domain
            CreateMap<ApplicationUserVM, ApplicationUser>();           
        }
    }
}
