using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Model.ViewModels;

namespace ExodusKorea.API.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public async Task<MainCurrenciesVM> GetMainCurrencies()
        {
            var httpClient = new HttpClient();
            var res = await httpClient.GetAsync($"https://api.exchangeratesapi.io/latest?symbols=USD,CAD,AUD,NZD&base=KRW");

            if (res.StatusCode != HttpStatusCode.OK)
                return null;

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(jsonRes);
            var usdRate = result.rates.USD.ToString();
            var cadRate = result.rates.CAD.ToString();
            var audRate = result.rates.AUD.ToString();
            var nzdRate = result.rates.NZD.ToString();

            var mainCurrenciesVM = new MainCurrenciesVM
            {
                USD = (1 / Convert.ToDecimal(usdRate)).ToString("#.##"),
                CAD = (1 / Convert.ToDecimal(cadRate)).ToString("#.##"),
                AUD = (1 / Convert.ToDecimal(audRate)).ToString("#.##"),
                NZD = (1 / Convert.ToDecimal(nzdRate)).ToString("#.##")
            };

            return mainCurrenciesVM;
        }

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
