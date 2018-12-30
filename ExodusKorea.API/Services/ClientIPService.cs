using System;
using ExodusKorea.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ExodusKorea.API.Services
{
    public class ClientIPService : IClientIPService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientIPService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClientIP(bool tryUseXForwardHeader = true)
        {
            string ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            // return if testing environment is localhost
            if (ip.Equals("::1"))
                return ip;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").Split(',').Select(s => s.Trim()).FirstOrDefault(); //client, proxy1, proxy2 and grab the client                  

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip == null || ip == "" && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip == null || ip == "")
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip == null || ip == "")
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (rawValues != null)
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public async Task<string> GetCountryCodeByIP(string ipAddress)
        {
            var country = "";
            var url = "http://api.ipstack.com/" + ipAddress + "?access_key=9d4f2a12307de0f196d758674051ea2d";
            var request = WebRequest.Create(url);

            using (WebResponse wrs = await request.GetResponseAsync())
            {
                using (Stream stream = wrs.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        var obj = JObject.Parse(json);
                        country = (string)obj["country_code"];                    
                    }
                }
            }

            return country;
        }
    }
}
