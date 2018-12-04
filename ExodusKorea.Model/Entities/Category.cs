using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<VideoPost> VideoPosts { get; set; }
    }
}