using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExodusKorea.API.Services
{
    public interface IGoogleRecaptchaService
    {
        Task<bool> ReCaptchaPassedAsync(string response, string secret);
    }
}
