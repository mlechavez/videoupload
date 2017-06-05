using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VideoUpload.Web.Models.Videos
{
    public class EditPostViewModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public int PostID { get; set; }        
        [Required]
        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }
        [AllowHtml]
        [Required]        
        public string Description { get; set; }
    }
}