using ExpenseTrackerWebApp.Models;

namespace ExpenseTrackerWebApp.Interfaces
{
    public interface IExpenseRepository
    {
        Task<ICollection<Expense>> GetAllExpenses();
        Task<Expense?> GetExpenseById(int? id);
        Task AddExpense(Expense expense);
        Task DeleteExpense(Expense expense);
        Task UpdateExpense(Expense expense);
    }
}
