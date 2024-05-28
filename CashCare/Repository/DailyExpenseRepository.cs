using CashCare.Data;
using CashCare.Interfaces;

namespace CashCare.Repository
{
    public class DailyExpenseRepository : IDailyExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public DailyExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal GetTotalExpenseThisMounth(int userId)
        {
            return 10;
        }

        public decimal GetTotalExpenseToday(int userId, int dayNumber)
        {
            var todayexpense = _context.ExpensesDaily.Where(data => data.AppUserId == userId && data.Date.Day == dayNumber && data.Date.Month == DateTime.Now.Month).Sum(data => data.Amount);
            return todayexpense;
        }
    }
}
