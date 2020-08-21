using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email Required")]
        [Display(Name="Email:")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Display(Name="Password:")]
        [Required(ErrorMessage="Password Required")]
        public string LoginPassword {get;set;}
    }
}