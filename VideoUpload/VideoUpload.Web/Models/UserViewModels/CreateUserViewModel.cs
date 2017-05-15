using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Employee no")]
        public string EmployeeNo { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }        
        public bool IsActive { get; set; }
        public string Password { get; set; }
        [Required]
        [Display(Name = "Email Password")]
        public string EmailPass { get; set; }
        [Required]
        [Display(Name = "Location")]
        public int? BranchID { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Direct line")]
        public string DirectLine { get; set; }
        [Required]
        [Display(Name = "Fax number")]
        public string FaxNumber { get; set; }
        [Required]
        [Display(Name = "Mobile number")]
        public string MobileNumber { get; set; }
        [Required]
        [Display(Name = "Work address")]
        public string WorkAddress { get; set; }
    }
}