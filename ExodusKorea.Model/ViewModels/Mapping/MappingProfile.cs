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
            CreateMap<VideoPost, VideoPostVM>()
                .ForMember(vm => vm.CategoryId, opt => opt.MapFrom(vp => vp.CategoryId))
                .ForMember(vm => vm.CountryEN, opt => opt.MapFrom(vp => vp.Country.NameEN))
                .ForMember(vm => vm.CountryKR, opt => opt.MapFrom(vp => vp.Country.NameKR))
                .ForMember(vm => vm.Category, opt => opt.MapFrom(vp => vp.Category.Name));
            CreateMap<CountryInfo, CountryInfoVM>()
                .ForMember(vm => vm.CountryEN, opt => opt.MapFrom(vp => vp.Country.NameEN))
                .ForMember(vm => vm.CountryKR, opt => opt.MapFrom(vp => vp.Country.NameKR));
            CreateMap<SalaryInfo, SalaryInfoVM>();
            CreateMap<PI_Etc, PI_EtcVM>();
            CreateMap<PI_Restaurant, PI_RestaurantVM>();
            CreateMap<PI_Rent, PI_RentVM>();
            CreateMap<PI_Groceries, PI_GroceriesVM>();
            CreateMap<Notification, NotificationVM>();
            CreateMap<MinimumCostOfLiving, MinimumCostOfLivingVM>();
            CreateMap<News, NewsVM>();
            CreateMap<NewsDetail, NewsDetailVM>();
            CreateMap<Career, CareerInfoVM>();
            CreateMap<JobSite, JobSiteVM>();
            CreateMap<VideoComment, VideoCommentVM>()
               .ForMember(vm => vm.CountryEN, opt => opt.MapFrom(vc => vc.Country))
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
                           CountryEN = vcr.Country
                       })));
            CreateMap<VideoCommentReply, VideoCommentReplyVM>()
                .ForMember(vm => vm.CountryEN, opt => opt.MapFrom(vc => vc.Country));
            // ViewModel to Domain
            CreateMap<ApplicationUserVM, ApplicationUser>();
            CreateMap<VideoPostVM, VideoPost>();
            CreateMap<CountryInfoVM, CountryInfo>();
            CreateMap<VideoCommentVM, VideoComment>();
        }
    }
}
