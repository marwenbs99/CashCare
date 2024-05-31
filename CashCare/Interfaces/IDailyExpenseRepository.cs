using CashCare.Models;

namespace CashCare.Interfaces
{
    public interface IDailyExpenseRepository
    {
        public decimal GetTotalExpenseToday(int userId, DateTime dayNumber);
        public IList<DailyExpense> GetListofExpenseThisDay(int userId, DateTime dateOfTheMonth);
    }
}
