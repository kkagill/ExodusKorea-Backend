using ExodusKorea.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ExodusKorea.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using ExodusKorea.Model.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using ExodusKorea.Model.ViewModels;

namespace ExodusKorea.API.Services
{
    public class LogDataService: ILogDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IClientIPService _clientIP;
        private readonly IMessageService _email;
        private readonly ExodusKoreaContext _context;
        private readonly HttpContext HttpContext;

        public LogDataService(IHttpContextAccessor httpContextAccessor,
                              IConfiguration config,
                              IClientIPService clientIP,
                              IMessageService email,
                              ExodusKoreaContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _clientIP = clientIP;
            _email = email;
            _context = context;

            HttpContext = _httpContextAccessor.HttpContext;
        }
        // Log 500 errors
        public async Task LogInternalServerException(ExceptionContext context)
        {
            string username = "UNKNOWN";
            string userId = "ANONYMOUS";
          
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userId = HttpContext.User.Identity.Name;
                username = _context.Users.Where(x => x.Id.Equals(userId)).SingleOrDefault().Email;
            }

            string domain = HttpContext.Request.Host.Value;
            string pageUrl = string.Format("{0}://{1}{2}{3}",
                             HttpContext.Request.Scheme,
                             HttpContext.Request.Host,
                             HttpContext.Request.Path,
                             HttpContext.Request.QueryString);
            string ipAddress = _clientIP.GetClientIP();

            try
            {
                await _context.SiteException.AddAsync(new SiteException
                {
                    CreateDate = DateTime.UtcNow,
                    Exception = context.Exception.ToString(),
                    IpAddress = ipAddress,
                    PageUrl = pageUrl,
                    UserId = userId
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _email.SendEmailAsync("admin@talchoseon.com", "admin@talchoseon.com", "[탈조선] SiteException 추가 오류", ex.ToString(), null);
            }

            string subject = "[탈조선] 500 오류";
            string body = "Domain: " + domain + "\r\n\r\n"
                           + "Page Url: " + pageUrl + "\r\n\r\n"
                           + "Username: " + username + "\r\n\r\n"
                           + "IP Address: " + ipAddress + "\r\n\r\n"
                           + "Exception: " + context.Exception.Message + "\r\n\r\n"
                           + "StackTrace: " + context.Exception.StackTrace + "\r\n\r\n"
                           + "Inputs: " + "\r\n\r\n";

            if (HttpContext.Request.HasFormContentType)
            {
                var form = HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                foreach (KeyValuePair<string, string> entry in form)
                    body += "Control: " + entry.Key + "; Value: " + entry.Value + "\r\n";
            }

            //await _email.SendEmailAsync("admin@talchoseon.com", "admin@talchoseon.com", subject, body, null);
        }
        // Log 404 or 400 errors
        public async Task LogHttpResponseException(HttpResponseExceptionVM vm)
        {           
            string domain = HttpContext.Request.Host.Value;         
            string ipAddress = _clientIP.GetClientIP();

            try
            {
                await _context.HttpResponseException.AddAsync(new HttpResponseException
                {
                    CreateDate = DateTime.UtcNow,
                    Status = vm.Status,
                    Error = vm.Error,
                    IpAddress = ipAddress,
                    Message = vm.Message,
                    UserId = vm.UserId ?? "ANONYMOUS"
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 이멜 너무 많이 들어올까봐 커맨트아웃함
                //await _email.SendEmailAsync("admin@exoduscorea.com", "admin@exoduscorea.com", "[엑소더스 코리아] HttpResponseException 추가 오류", ex.ToString(), null);
            }
            // Skip sending email notification for Not Found 404 error
            //if (vm.Status != 404)
            //{
            //    vm.Username = vm.Username ?? "ANONYMOUS";

            //    string subject = "[엑소더스 코리아] " + vm.Status + " 오류";
            //    string body = "Domain: " + domain + "\r\n\r\n"
            //                   + "Status: " + vm.Status + "\r\n\r\n"
            //                   + "Username: " + vm.Username + "\r\n\r\n"
            //                   + "IP Address: " + ipAddress + "\r\n\r\n"
            //                   + "Error: " + vm.Error + "\r\n\r\n"
            //                   + "Message: " + vm.Message + "\r\n\r\n"
            //                   + "Inputs: " + "\r\n\r\n";

            //    if (HttpContext.Request.HasFormContentType)
            //    {
            //        var form = HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            //        foreach (KeyValuePair<string, string> entry in form)
            //            body += "Control: " + entry.Key + "; Value: " + entry.Value + "\r\n";
            //    }

            //    await _email.SendEmailAsync("admin@exoduscorea.com", "admin@exoduscorea.com", subject, body, null);
            //}           
        }

        public async Task LogLoginSession(ApplicationUser user, string loginType)
        {           
            string ipAddress = _clientIP.GetClientIP();

            try
            {
                await _context.LoginSession.AddAsync(new LoginSession
                {
                   UserId = user.Id,
                   UserName = user.Email,
                   IpAddress = ipAddress,
                   LoginTime = DateTime.UtcNow,
                   LoginType = loginType
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                await _email.SendEmailAsync("admin@talchoseon.com", "admin@talchoseon.com", "[탈조선] LoginSession 추가 오류", ex.ToString(), null);
            }
        }      
    }
}
