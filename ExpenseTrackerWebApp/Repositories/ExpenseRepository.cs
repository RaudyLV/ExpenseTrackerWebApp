using ExpenseTrackerWebApp.Data;
using ExpenseTrackerWebApp.Interfaces;
using ExpenseTrackerWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpenseTrackerWebApp.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext appDbContext;

        public ExpenseRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddExpense(Expense expense)
        {
            await appDbContext.AddAsync(expense);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteExpense(Expense expense)
        {
            appDbContext.Expenses.Remove(expense);
            await appDbContext.SaveChangesAsync();
        }
        public async Task UpdateExpense(Expense expense)
        {
            appDbContext.Expenses.Update(expense);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Expense>> GetAllExpenses() => await appDbContext.Expenses.ToListAsync();

        public async Task<Expense?> GetExpenseById(int? id)
        {
			if (id == null)
			{
				Debug.WriteLine("GetExpenseById: El id es null");
				return null;
			}
			var expense = await appDbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            return expense;
        }

    }
}
