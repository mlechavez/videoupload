using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.UserViewModels
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Designation")]
        public string JobTitle { get; set; }        
        [Display(Name = "Employe no")]
        public string EmployeeNo { get; set; }
        public string Email { get; set; }      
        public bool IsActive { get; set; }
        [Display(Name = "Email password")]
        public string EmailPass { get; set; }
    }
}