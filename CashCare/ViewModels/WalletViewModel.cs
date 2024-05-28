using CashCare.Models.Wallet;
using CashCare.ViewModels.Enum;

namespace CashCare.ViewModels
{
    public class WalletViewModel
    {
        public Models.Wallet.Wallet Wallet { get; set; } = new Models.Wallet.Wallet();
        public ButtonActionType BtnAction { get; set; }
        public Income Income { get; set; }
        public Debt Debt { get; set; }
        public Expense Expense { get; set; }
        public MenuStateVM MenuState { get; set; }
        public Saving Saving { get; set; } = new Saving();

        public decimal TotalIncome
        {
            get
            {
                return Wallet?.Incomes?.Sum(i => i.Amount) ?? 0;
            }
        }

        public decimal TotalDebts
        {
            get
            {
                return Wallet?.Debts?.Sum(i => i.Amount) ?? 0;
            }
        }

        public decimal TotalExpense
        {
            get
            {
                return Wallet?.ExpenseListe?.Sum(i => i.Amount) ?? 0;
            }
        }

        public decimal NettIncomeAfter
        {
            get
            {
                return TotalIncome - TotalDebts - TotalExpense;
            }
        }
    }
}
