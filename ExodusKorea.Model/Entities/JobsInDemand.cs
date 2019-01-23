﻿using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class JobsInDemand
    {
        public int JobsInDemandId { get; set; }
        public string TitleEN { get; set; }
        public string TitleKR { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<VideoPost> VideoPosts { get; set; }
    }
}