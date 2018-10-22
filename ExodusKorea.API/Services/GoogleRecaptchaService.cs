using ExodusKorea.API.Exceptions;
using ExodusKorea.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

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
