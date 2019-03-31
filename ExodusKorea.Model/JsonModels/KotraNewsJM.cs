using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExodusKorea.Model.JsonModels
{
    public partial class KotraNewsJM
    {
        [JsonProperty("pageNo")]
        public long PageNo { get; set; }

        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("items")]
        public Item_KotraNews[] Items { get; set; }

        [JsonProperty("numOfRows")]
        public long NumOfRows { get; set; }

        [JsonProperty("resultMsg")]
        public string ResultMsg { get; set; }
    }

    public partial class Item_KotraNews
    {
        [JsonProperty("bbstxSnDatatype")]
        public string BbstxSnDatatype { get; set; }

        [JsonProperty("newsTitl")]
        public string NewsTitl { get; set; }

        [JsonProperty("newsWrterNmDatatype")]
        public string NewsWrterNmDatatype { get; set; }

        [JsonProperty("jobSeNm")]
        public string JobSeNm { get; set; }

        [JsonProperty("natnDatatype")]
        public string NatnDatatype { get; set; }

        [JsonProperty("indstClDatatype")]
        public string IndstClDatatype { get; set; }

        [JsonProperty("regnDatatype")]
        public string RegnDatatype { get; set; }

        [JsonProperty("regDtDatatype")]
        public string RegDtDatatype { get; set; }

        [JsonProperty("crrspOvrofLink")]
        public string CrrspOvrofLink { get; set; }

        [JsonProperty("newsBdtDatatype")]
        public string NewsBdtDatatype { get; set; }

        [JsonProperty("indstCl")]
        public string IndstCl { get; set; }

        [JsonProperty("newsBdt")]
        public string NewsBdt { get; set; }

        [JsonProperty("newsWrtDt")]
        public DateTimeOffset NewsWrtDt { get; set; }

        [JsonProperty("newsTitlDatatype")]
        public string NewsTitlDatatype { get; set; }

        [JsonProperty("bbstxSn")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long BbstxSn { get; set; }

        [JsonProperty("inqreCnt")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long InqreCnt { get; set; }

        [JsonProperty("infoCl")]
        public string InfoCl { get; set; }

        [JsonProperty("jobSeNmDatatype")]
        public string JobSeNmDatatype { get; set; }

        [JsonProperty("updDtDatatype")]
        public string UpdDtDatatype { get; set; }

        [JsonProperty("natn")]
        public string Natn { get; set; }

        [JsonProperty("inqryItmLinkDatatype")]
        public string InqryItmLinkDatatype { get; set; }

        [JsonProperty("infoClDatatype")]
        public string InfoClDatatype { get; set; }

        [JsonProperty("newsWrtDtDatatype")]
        public string NewsWrtDtDatatype { get; set; }

        [JsonProperty("inqreCntDatatype")]
        public string InqreCntDatatype { get; set; }

        [JsonProperty("kwrdDatatype")]
        public string KwrdDatatype { get; set; }

        [JsonProperty("kwrd")]
        public string Kwrd { get; set; }

        [JsonProperty("regn")]
        public string Regn { get; set; }

        [JsonProperty("regDt")]
        public DateTimeOffset RegDt { get; set; }

        [JsonProperty("newsBdtDatanotice")]
        public string NewsBdtDatanotice { get; set; }

        [JsonProperty("cntntSumar")]
        public string CntntSumar { get; set; }

        [JsonProperty("ovrofInfoDatatype")]
        public string OvrofInfoDatatype { get; set; }

        [JsonProperty("ovrofInfo")]
        public string OvrofInfo { get; set; }

        [JsonProperty("inqryItmLink")]
        public string InqryItmLink { get; set; }

        [JsonProperty("newsWrterNm")]
        public string NewsWrterNm { get; set; }

        [JsonProperty("updDt")]
        public DateTimeOffset UpdDt { get; set; }

        [JsonProperty("cntntSumarDatatype")]
        public string CntntSumarDatatype { get; set; }

        [JsonProperty("crrspOvrofLinkDatatype")]
        public string CrrspOvrofLinkDatatype { get; set; }
    }

    public partial class KotraNewsJM
    {
        public static KotraNewsJM FromJson(string json) => JsonConvert.DeserializeObject<KotraNewsJM>(json, Converter.Settings);
    }

    public static class SerializeKotraNews
    {
        public static string ToJson(this KotraNewsJM self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class ConverterKotraNews
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}