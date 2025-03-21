using ExpenseTrackerWebApp.Interfaces;
using ExpenseTrackerWebApp.Models;
using ExpenseTrackerWebApp.ViewModels;
using System.Security.Claims;
namespace ExpenseTrackerWebApp.Services
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExpenseService(IExpenseRepository expenseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _expenseRepository = expenseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddExpenseAsync(ExpenseVM expenseVM)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return false;
            }
            if (expenseVM.Amount < 0 || string.IsNullOrEmpty(expenseVM.Description))
            {
                return false;
            }

            var expense = new Expense
            {
                Amount = expenseVM.Amount,
                Description = expenseVM.Description,
                Date = DateTime.Now,
                userId = userId
            };

            await _expenseRepository.AddExpense(expense);
            return true;
        }
        public async Task<bool> DeleteExpense(int id)
        {
            var expense = await _expenseRepository.GetExpenseById(id);
            if (expense == null)
            {
                return false;
            }

            await _expenseRepository.DeleteExpense(expense);
            return true;
        }

        public async Task<bool> UpdateAsync(ExpenseVM expenseVM)
        {
            var expense = await _expenseRepository.GetExpenseById(expenseVM.Id);
            if (expense == null)
            {
                return false;
            }

            expense.Description = expenseVM.Description;
            expense.Date = DateTime.Now;
            expense.Amount = expenseVM.Amount;

            await _expenseRepository.UpdateExpense(expense);
            return true;
        }

        public async Task<ICollection<Expense>> GetAllExpense() => await _expenseRepository.GetAllExpenses();

        public async Task<Expense?> GetById(int id) => await _expenseRepository.GetExpenseById(id);


        #region Extra methods
        public async Task<List<ExpenseGroupVM>> GetExpensesGroupedByMonth(string userId)
        {
            var expenses = await _expenseRepository.GetAllExpenses();
            var userExpenses = expenses.Where(e => e.userId == userId);

            var grouped = userExpenses
                .GroupBy(e => e.Date.Month)
                .Select(g => new ExpenseGroupVM
                {
                    GroupKey = g.Key,
                    ExpenseDescription = g.Select(e => e.Description).ToList(),
                    ExpenseDate = g.Select(e => e.Date).ToList(),
                    ExpenseAmount = g.Select(e => e.Amount).ToList()
                }).ToList();

            return grouped;
        }

        public async Task<List<ExpenseGroupVM>> GetExpensesGroupedByYear(string userId)
        {
            var expenses = await _expenseRepository.GetAllExpenses();
            var userExpenses = expenses.Where(e => e.userId == userId);

            var grouped = userExpenses
                .GroupBy(e => e.Date.Year)
                .Select(g => new ExpenseGroupVM
                {
                    GroupKey = g.Key,
                    ExpenseDescription = g.Select(e => e.Description).ToList(),
                    ExpenseDate = g.Select(e => e.Date).ToList(),
                    ExpenseAmount = g.Select(e => e.Amount).ToList()
                }).ToList();

            return grouped;
        }

        #endregion

    }
}
