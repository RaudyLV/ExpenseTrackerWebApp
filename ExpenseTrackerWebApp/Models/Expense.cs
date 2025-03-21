using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerWebApp.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public  required string? Description { get; set; }
        public required double Amount { get; set; }
        public required DateTime Date { get; set; } = DateTime.Now;

        //FK
        public string userId {  get; set; }
        public virtual User User { get; set; } 
    }
}
