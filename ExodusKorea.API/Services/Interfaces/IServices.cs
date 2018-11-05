using ExodusKorea.Model.Entities;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExodusKorea.API.Services.Interfaces
{
    public interface ICurrencyRatesService
    {
        Task<string> GetKRWRateByCountry(string baseCurrency);
    }

    public interface IGoogleRecaptchaService
    {
        Task<bool> ReCaptchaPassedAsync(string response, string secret);
    }

    public interface IMessageService
    {
        Task SendEmailAsync(string email, string subject, string message, string htmlBody);
    }

    public interface IYouTubeService
    {
        Task<string> GetYouTubeLikesByVideoId(string videoId);
        Task<YouTubeCommentVM> GetYouTubeCommentsByVideoId(string videoId);
    }
}