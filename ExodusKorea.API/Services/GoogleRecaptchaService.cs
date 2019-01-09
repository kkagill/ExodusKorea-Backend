using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using ExodusKorea.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using ExodusKorea.Model;

namespace ExodusKorea.API.Services
{
    public class GoogleRecaptchaService : IGoogleRecaptchaService
    {
        private readonly AppSettings _appSettings;
        private readonly GoogleReCaptcha _gReCaptchaAccessor;
        private readonly GoogleReCaptchaDev _gReCaptchaDevAccessor;

        public GoogleRecaptchaService(IOptions<AppSettings> appSettings,
            IOptionsSnapshot<GoogleReCaptcha> gReCaptchaAccessor,
            IOptionsSnapshot<GoogleReCaptchaDev> gReCaptchaDevAccessor)
        {
            _appSettings = appSettings.Value;
            _gReCaptchaAccessor = gReCaptchaAccessor.Value;
            _gReCaptchaDevAccessor = gReCaptchaDevAccessor.Value;
        }

        public async Task<bool> ReCaptchaPassedAsync(string response)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage res = null;

            if (_appSettings.Environment.Equals("Live"))
                res = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_gReCaptchaAccessor.Secret}&response={response}");
            else if (_appSettings.Environment.Equals("Development"))
                res = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_gReCaptchaDevAccessor.Secret}&response={response}");

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
