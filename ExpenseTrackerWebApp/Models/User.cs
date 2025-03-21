using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerWebApp.Models
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "The full name is required!")]
        public string FullName { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
