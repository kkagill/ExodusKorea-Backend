using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class SalaryInfo
    {
        public int SalaryInfoId { get; set; }
        public string Country { get; set; }
        public string Occupation { get; set; }
        public string Currency { get; set; }
        public decimal Low { get; set; }
        public decimal Median { get; set; }
        public decimal High { get; set; }
        public bool IsDisplayable { get; set; }
        //public int VideoPostId { get; set; }
        public ICollection<VideoPost> VideoPosts { get; set; }
    }
}