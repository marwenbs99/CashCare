using CashCare.Models.Wallet.Enum;

namespace CashCare.Models.Wallet
{
    public class Expense
    {
        public ExpenseType TypeOfExpense { get; set; }
        public decimal Amount { get; set; }
    }
}
