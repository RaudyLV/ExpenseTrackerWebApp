namespace ExpenseTrackerWebApp.ViewModels
{
    public class ExpenseGroupVM
    {
        public int GroupKey { get; set; }
        public List<string?> ExpenseDescription { get; set; }
        public List<DateTime> ExpenseDate { get; set; }
        public List<double> ExpenseAmount { get; set; }
    }
}
