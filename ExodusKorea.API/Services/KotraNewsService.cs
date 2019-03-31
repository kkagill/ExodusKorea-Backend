using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System;
using ExodusKorea.Model.JsonModels;
using ExodusKorea.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using ExodusKorea.Model;
using ExodusKorea.Model.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace ExodusKorea.API.Services
{
    public class KotraNewsService : IKotraNewsService
    {
        private readonly KotraNews _serviceKey;
        private readonly IMemoryCache _memoryCache;

        public KotraNewsService(IOptionsMonitor<KotraNews> kotraNewsAccessor,
                                IMemoryCache memoryCache)
        {
            _serviceKey = kotraNewsAccessor.CurrentValue;
            _memoryCache = memoryCache;
        }    

        public async Task<List<KotraNewsVM>> GetKotraNewsByCountry(string country)
        {
            // https://stackoverflow.com/questions/19883524/why-is-httpclients-getstringasync-is-unbelivable-slow
            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            var res = await httpClient
                .GetAsync($"http://apis.data.go.kr/B410001/ovseaMrktNewsService/ovseaMrktNews?serviceKey={_serviceKey.Key}&type=json&numOfRows=50&pageNo=1&search1={country}");
            
            if (res.StatusCode != HttpStatusCode.OK)
                return null;

            var jsonRes = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<KotraNewsJM>(jsonRes);
            var kotraNewsVM = new List<KotraNewsVM>();

            if (result != null)
                foreach (var item in result.Items)
                    kotraNewsVM.Add(new KotraNewsVM
                    {
                        NewsTitl = item.NewsTitl,
                        NewsBdt = item.NewsBdt,
                        NewsWrtDt = item.NewsWrtDt,
                        NewsWrterNm = item.NewsWrterNm,
                        OvrofInfo = item.OvrofInfo
                    });

            // Store in memory cache so we don't have to access api everytime
            // Deletes cache after 1 day
            var cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1),
                Priority = CacheItemPriority.Normal
            };

            _memoryCache.Set(country, kotraNewsVM, cacheExpirationOptions);
           
            return kotraNewsVM;
        }
    }
}
