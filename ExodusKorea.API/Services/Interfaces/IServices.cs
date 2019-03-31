using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.API.Services.Interfaces
{
    public interface ICurrencyRatesService
    {
        Task<MainCurrenciesVM> GetMainCurrencies();
        Task<string> GetKRWRateByCountry(string baseCurrency);
    }

    public interface IGoogleRecaptchaService
    {
        Task<bool> ReCaptchaPassedAsync(string response);
    }

    public interface IMessageService
    {
        Task SendEmailAsync(string from, string to, string subject, string message, string htmlBody);
    }

    public interface IYouTubeService
    {
        Task<YouTubeChannelInfoVM> GetYouTubeChannelInfoByChannelId(string channelId);
        Task<YouTubeInfoVM> GetYouTubeInfoByVideoId(string videoId);
        Task<YouTubeCommentVM> GetYouTubeCommentsByVideoId(string videoId);
        Task<List<Reply>> GetYouTubeRepliesByParentId(string parentId);
    }

    public interface IClientIPService
    {
        string GetClientIP(bool tryUseXForwardHeader = true);
        Task<string> GetCountryCodeByIP(string ipAddress);
    }

    public interface ILogDataService
    {
        Task LogInternalServerException(ExceptionContext context);
        Task LogHttpResponseException(HttpResponseExceptionVM vm);
        Task LogLoginSession(ApplicationUser user, string loginType);
    }

    public interface IKotraNewsService
    {
        Task<List<KotraNewsVM>> GetKotraNewsByCountry(string country);
    }
}