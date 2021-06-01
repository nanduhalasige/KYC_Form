using System.ComponentModel.DataAnnotations;

namespace KYC_Form.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Employee First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Employee Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Employee Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [MaxLength(10)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Client Type")]
        public string ClientType { get; set; }

        [Required(ErrorMessage = "Select any Title")]
        public string Title { get; set; }

        public string Gender { get; set; }
    }
}