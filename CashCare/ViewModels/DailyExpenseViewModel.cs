using CashCare.Models;

namespace CashCare.ViewModels
{
    public class DailyExpenseViewModel
    {
        public ICollection<DailyExpense> ExpensesDaily { get; set; }
        public DailyExpense currentExpense { get; set; }
        public WalletViewModel currentWallet { get; set; } = new WalletViewModel();
        public decimal todaytotalExpense { get; set; } = 0;
    }
}
