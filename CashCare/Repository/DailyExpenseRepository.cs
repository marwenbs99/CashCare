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
        public IList<DailyExpense> GetListofExpenseThisDay(int userId, DateTime dateOfTheMonth)
        {
            return _context.ExpensesDaily.Where(ex => ex.Date.Day == dateOfTheMonth.Day && ex.Date.Month == dateOfTheMonth.Month && ex.Date.Year == dateOfTheMonth.Year && ex.AppUserId == userId).ToList();
        }

        public decimal GetTotalExpenseToday(int userId, DateTime dayNumber)
        {
            var todayexpense = _context.ExpensesDaily.Where(data => data.AppUserId == userId && data.Date.Day == dayNumber.Day && data.Date.Month == dayNumber.Month).Sum(data => data.Amount);
            return todayexpense;
        }
    }
}
