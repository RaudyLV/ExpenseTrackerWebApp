using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerWebApp.ViewModels
{
    public class RegisterVM
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "The full name is required!")]
        [DisplayName("Full name")]
        public string FullName { get; set; }
        [MaxLength(20), Required(ErrorMessage ="The email is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage ="The password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "Password do not match!")]
        public string ConfirmPassword { get; set; }

    }
}
