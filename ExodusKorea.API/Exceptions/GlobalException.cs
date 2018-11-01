using ExodusKorea.API.Services;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExodusKorea.API.Exceptions
{
    public class GlobalException : ExceptionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly ExodusKoreaContext _context;
        private readonly IMessageService _email;
        private readonly HttpContext HttpContext;

        public GlobalException(IHttpContextAccessor httpContextAccessor,
                               IConfiguration config,
                               ExodusKoreaContext context,
                               IMessageService email)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _context = context;
            _email = email;

            HttpContext = _httpContextAccessor.HttpContext;
        }

        public void HandleException(string ex, string message, string stackTrace)
        {
            string username = "UNKNOWN";
            string userID = "ANONYMOUS";

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                username = HttpContext.User.Identity.Name;
                userID = _context.Users.Where(x => x.UserName.Equals(username)).SingleOrDefault().Id;
            }

            string domain = HttpContext.Request.Host.Value;
            string pageUrl = string.Format("{0}://{1}{2}{3}",
                             HttpContext.Request.Scheme,
                             HttpContext.Request.Host,
                             HttpContext.Request.Path,
                             HttpContext.Request.QueryString);

            string exceptionMsg = message;
            string exceptionStackTrace = stackTrace;
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            // Log in database if in live site
            int exceptionID = -1;
            try
            {
                exceptionID = LogException(pageUrl, userID, ex, ipAddress);
            }
            catch (Exception e)
            {
                _email.SendEmailAsync("kkagill@gmail.com", "*** 엑소더스 코리아 오류 (LogException) ***", e.ToString(), null);
            }
        
            string subject = "*** 엑소더스 코리아 오류 ***";
            string body = "Domain: " + domain + "\r\n\r\n"
                           + "Page Url: " + pageUrl + "\r\n\r\n"
                           + "Username: " + username + "\r\n\r\n"                      
                           + "IP Address: " + ipAddress + "\r\n\r\n"
                           + "ExceptionID: " + exceptionID.ToString() + "\r\n\r\n"
                           + "Exception: " + exceptionMsg + "\r\n\r\n"
                           + "StackTrace: " + exceptionStackTrace + "\r\n\r\n"
                           + "Inputs: " + "\r\n\r\n";

            if (HttpContext.Request.HasFormContentType)
            {
                var form = HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

                foreach (KeyValuePair<string, string> entry in form)
                {
                    body += "Control: " + entry.Key + "; Value: " + entry.Value + "\r\n";
                }

                //or https://books.google.ca/books?id=kVzKCQAAQBAJ&pg=PA1241&lpg=PA1241&dq=HasFormContentType&source=bl&ots=CHk0a_oqEP&sig=zZg0xJxtJXTCxgCFtgKz1D5nh7Q&hl=en&sa=X&ved=0ahUKEwjw8_aVhKDTAhVG2WMKHfgBCTEQ6AEIUzAI#v=onepage&q=HasFormContentType&f=false
                //IFormCollection coll = HttpContext.Request.Form;
                //foreach (var key in coll.Keys)
                //{
                //    builder.HtmlBody += "control: " + key + "; value: " + HttpContext.Request.Form[key].ToString() + "\r\n";
                //}
            }

            _email.SendEmailAsync("kkagill@gmail.com", subject, body, null);
        }

        private int LogException(string pageUrl, string userID, string exception, string ipAddress)
        {
            int exceptionID = -1;
          
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ExodusKorea")))
            {
                var cmd = new SqlCommand("dbo.stp_LOG_AddSiteException", conn) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("PageUrl", pageUrl));
                cmd.Parameters.Add(new SqlParameter("UserID", userID));
                cmd.Parameters.Add(new SqlParameter("Exception", exception));
                cmd.Parameters.Add(new SqlParameter("IpAddress", ipAddress));

                cmd.Parameters.Add("@ExceptionID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                conn.Open();
                cmd.ExecuteNonQuery();

                int.TryParse(cmd.Parameters["@ExceptionID"].Value.ToString(), out exceptionID);

                conn.Close();

                return exceptionID;
            }
        }
    }
}
