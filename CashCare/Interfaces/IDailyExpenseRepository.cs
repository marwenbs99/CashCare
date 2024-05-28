namespace CashCare.Interfaces
{
    public interface IDailyExpenseRepository
    {
        public decimal GetTotalExpenseToday(int userId, int dayNumber);
        public decimal GetTotalExpenseThisMounth(int userId);
    }
}
