using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerWebApp.ViewModels
{
    public class ExpenseVM
    {
        public int Id { get; set; }
        public required string? Description { get; set; }
        public required double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public required DateTime Date { get; set; } = DateTime.Now;
    }
}
