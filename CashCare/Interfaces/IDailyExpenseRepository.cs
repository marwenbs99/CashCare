using CashCare.Models;

namespace CashCare.Interfaces
{
    public interface IDailyExpenseRepository
    {
        public decimal GetTotalExpenseToday(int userId, int dayNumber);
        public IList<DailyExpense> GetListofExpenseThisDay(int userId, int dateOfTheMonth);
    }
}
