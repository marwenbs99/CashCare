using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models;

namespace CashCare.Repository
{
    public class DailyExpenseRepository : IDailyExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public DailyExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<DailyExpense> GetListofExpenseThisDay(int userId, int dateOfTheMonth)
        {
            return _context.ExpensesDaily.Where(ex => ex.Date.Day == dateOfTheMonth && ex.Date.Month == DateTime.Now.Month && ex.Date.Year == DateTime.Now.Year).ToList();
        }

        public decimal GetTotalExpenseToday(int userId, int dayNumber)
        {
            var todayexpense = _context.ExpensesDaily.Where(data => data.AppUserId == userId && data.Date.Day == dayNumber && data.Date.Month == DateTime.Now.Month).Sum(data => data.Amount);
            return todayexpense;
        }
    }
}
