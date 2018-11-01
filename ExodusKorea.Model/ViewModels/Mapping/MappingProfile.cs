using AutoMapper;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;

namespace ExodusKorea.Model.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to ViewModel
            CreateMap<ApplicationUser, ApplicationUserVM>();
            CreateMap<NewVideo, NewVideosVM>();
            CreateMap<CountryInfo, CountryInfoVM>();
            // ViewModel to Domain
            CreateMap<ApplicationUserVM, ApplicationUser>();
            CreateMap<NewVideosVM, NewVideo>();
            CreateMap<CountryInfoVM, CountryInfo>();
        }
    }
}
