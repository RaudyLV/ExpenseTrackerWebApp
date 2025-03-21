using ExpenseTrackerWebApp.Services;
using ExpenseTrackerWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerWebApp.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ExpenseService _expenseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExpenseController(ExpenseService expenseService, IHttpContextAccessor httpContextAccessor)
        {
            _expenseService = expenseService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> IndexAsync(string groupBy, int? month, int? year)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Asigna los meses y años al ViewBag para los filtros
            ViewBag.Months = Enumerable.Range(1, 12)
                .Select(m => new { Month = m, MonthName = new DateTime(2021, m, 1).ToString("MMMM") })
                .ToList();

            ViewBag.Years = Enumerable.Range(2020, DateTime.Now.Year - 2019).ToList(); // Ejemplo

            IEnumerable<ExpenseGroupVM> model;

            // Según el parámetro groupBy, decides cómo agrupar
            if (!string.IsNullOrEmpty(groupBy) && groupBy.ToLower() == "year")
            {
                // Si se pasa un año, podrías filtrar también
                if (year.HasValue)
                {
                    // Filtra por año y agrupa (puedes agregar el filtro en el servicio)
                    model = await _expenseService.GetExpensesGroupedByYear(userId);
                    model = model.Where(g => g.GroupKey == year.Value).ToList();
                }
                else
                {
                    model = await _expenseService.GetExpensesGroupedByYear(userId);
                }
            }
            else
            {
                // Por defecto, agrupamos por mes
                if (month.HasValue)
                {
                    model = await _expenseService.GetExpensesGroupedByMonth(userId);
                    model = model.Where(g => g.GroupKey == month.Value).ToList();
                }
                else
                {
                    model = await _expenseService.GetExpensesGroupedByMonth(userId);
                }
            }

            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseVM expenseVM)
        {
            if (ModelState.IsValid)
            {
                await _expenseService.AddExpenseAsync(expenseVM);
                return RedirectToAction("Index");
            }

            return View(expenseVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetById(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expenseService.DeleteExpense(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetById(id);
            if (expense == null)
            {
                return NotFound();
            }

            var expenseVM = new ExpenseVM
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date
            };

            return View(expenseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseVM expenseVM)
        {
            if (ModelState.IsValid)
            {
                var success = await _expenseService.UpdateAsync(expenseVM);

                if (success)
                    return RedirectToAction("Index");
            }
            return View(expenseVM);
        }
    }
}
