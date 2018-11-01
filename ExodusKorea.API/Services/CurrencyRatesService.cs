using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System;
using ExodusKorea.API.Services.Interfaces;

namespace ExodusKorea.API.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {      
        public async Task<string> GetKRWRateByCountry(string baseCurrency)
        {
            var httpClient = new HttpClient();
            var res = await httpClient.GetAsync($"https://api.exchangeratesapi.io/latest?base={baseCurrency}");

            if (res.StatusCode != HttpStatusCode.OK)
                return "";

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(jsonRes);
            var krwRate = result.rates.KRW.ToString();

            if (string.IsNullOrEmpty(krwRate))
                return "";
           
            return Convert.ToDecimal(krwRate).ToString("#.##");
        }
    }
}
