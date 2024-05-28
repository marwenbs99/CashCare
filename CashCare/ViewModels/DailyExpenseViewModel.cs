using CashCare.Models;

namespace CashCare.ViewModels
{
    public class DailyExpenseViewModel
    {
        public IList<ExpensesPerDayViewModel> ListexpensePerDay { get; set; } = new List<ExpensesPerDayViewModel>();
        public DailyExpense currentExpense { get; set; }
        public WalletViewModel currentWallet { get; set; } = new WalletViewModel();
        public decimal todaytotalExpense { get; set; } = 0;
    }
}
