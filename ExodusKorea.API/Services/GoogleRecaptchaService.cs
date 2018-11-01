using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using ExodusKorea.API.Services.Interfaces;

namespace ExodusKorea.API.Services
{
    public class GoogleRecaptchaService : IGoogleRecaptchaService
    {      
        public async Task<bool> ReCaptchaPassedAsync(string response, string secret)
        {
            var httpClient = new HttpClient();
            var res = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={response}");

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string jsonRes = await res.Content.ReadAsStringAsync();
            dynamic jsonData = JObject.Parse(jsonRes);

            if (jsonData.success != "true")
                return false;

            return true;
        }
    }
}
