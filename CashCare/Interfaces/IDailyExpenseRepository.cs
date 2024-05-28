namespace CashCare.Interfaces
{
    public interface IDailyExpenseRepository
    {
        public decimal GetTotalExpenseToday(int userId);
        public decimal GetTotalExpenseThisMounth(int userId);
    }
}
