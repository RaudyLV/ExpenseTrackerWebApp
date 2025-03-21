using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerWebApp.ViewModels
{
    public class LoginVM
    {
        [MaxLength(20), Required(ErrorMessage = "The email is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool IsPersistent { get; set; }
    }
}
