using CashCare.Models.Wallet;

namespace CashCare.Interfaces
{
    public interface IwalletRepository
    {
        public Income GetIncomeById(int id);
        public Debt GetDebtById(int id);
        public Expense GetExpenseById(int id);
        public Wallet GetCurrentWallet(int id);
    }
}
