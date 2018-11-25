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
            CreateMap<VideoPost, VideoPostVM>();
            CreateMap<CountryInfo, CountryInfoVM>();
            CreateMap<SalaryInfo, SalaryInfoVM>();
            CreateMap<PI_Etc, PI_EtcVM>();
            CreateMap<PI_Restaurant, PI_RestaurantVM>();
            CreateMap<PI_Rent, PI_RentVM>();
            CreateMap<PI_Groceries, PI_GroceriesVM>();
            CreateMap<Notification, NotificationVM>();
            CreateMap<MinimumCostOfLiving, MinimumCostOfLivingVM>();
            CreateMap<News, NewsVM>();
            CreateMap<NewsDetail, NewsDetailVM>();
            CreateMap<VideoComment, VideoCommentVM>()             
               .ForMember(vm => vm.VideoCommentReplies, 
               opt => opt.MapFrom(vc => vc.VideoCommentReplies.OrderBy(vcr => vcr.DateCreated)
               .Select(vcr => new VideoCommentReplyVM
               {
                   VideoCommentReplyId = vcr.VideoCommentReplyId,
                   VideoCommentId = vcr.VideoCommentId,
                   AuthorDisplayName = vcr.AuthorDisplayName,
                   Comment = vcr.Comment,
                   DateCreated = vcr.DateCreated,
                   DateUpdated = vcr.DateUpdated,
                   Likes = vcr.Likes,
                   UserId = vcr.UserId,
                   RepliedTo = vcr.RepliedTo,
                   Country = vcr.Country
               })));
            // ViewModel to Domain
            CreateMap<ApplicationUserVM, ApplicationUser>();
            CreateMap<VideoPostVM, VideoPost>();
            CreateMap<CountryInfoVM, CountryInfo>();
            CreateMap<VideoCommentVM, VideoComment>();
        }
    }
}
