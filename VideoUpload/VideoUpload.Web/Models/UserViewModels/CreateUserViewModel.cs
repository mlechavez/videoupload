using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.UserViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Employee no")]
        public string EmployeeNo { get; set; }
        [Required]
        public string Email { get; set; }        
        public bool IsActive { get; set; }
        public string Password { get; set; }
        [Required]
        [Display(Name = "Email Password")]
        public string EmailPass { get; set; }
    }
}