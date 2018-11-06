using AutoMapper;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;
using System.Linq;

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
            CreateMap<VideoComment, VideoCommentVM>()             
               .ForMember(vm => vm.VideoCommentReplies, 
               opt => opt.MapFrom(i => i.VideoCommentReplies.OrderByDescending(x => x.DateCreated)
               .Select(vcr => new VideoCommentReplyVM
               {
                   VideoCommentReplyId = vcr.VideoCommentReplyId,
                   VideoCommentId = vcr.VideoCommentId,
                   AuthorDisplayName = vcr.AuthorDisplayName,
                   Comment = vcr.Comment,
                   DateCreated = vcr.DateCreated,
                   DateUpdated = vcr.DateUpdated,
                   Likes = vcr.Likes                   
               })));
            // ViewModel to Domain
            CreateMap<ApplicationUserVM, ApplicationUser>();
            CreateMap<NewVideosVM, NewVideo>();
            CreateMap<CountryInfoVM, CountryInfo>();
            CreateMap<VideoCommentVM, VideoComment>();
        }
    }
}
