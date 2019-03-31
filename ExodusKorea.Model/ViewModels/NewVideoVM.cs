using System.ComponentModel.DataAnnotations;

namespace ExodusKorea.Model.ViewModels
{
    public class NewVideoVM
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int CountryId { get; set; }      
        public int? CareerId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int UploaderId { get; set; }
        [Required]
        public string YouTubeVideoId { get; set; }
        [Required]
        public int Likes { get; set; }
        public int? SalaryInfoId { get; set; }
        public string SharerId { get; set; }     
        public bool IsGoogleDriveVideo { get; set; }
    }
}