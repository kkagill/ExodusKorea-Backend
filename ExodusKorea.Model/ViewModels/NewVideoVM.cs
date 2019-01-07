using System.ComponentModel.DataAnnotations;

namespace ExodusKorea.Model.ViewModels
{
    public class NewVideoVM
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Uploader { get; set; }
        [Required]
        public string YouTubeVideoId { get; set; }
        public int? SalaryInfoId { get; set; }
        public long VimeoId { get; set; }
        public string SharerId { get; set; }
    }
}