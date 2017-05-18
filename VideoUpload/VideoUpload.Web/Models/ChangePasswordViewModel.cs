using System.ComponentModel.DataAnnotations;

namespace VideoUpload.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
}