using CashCare.Models.Wallet.Enum;

namespace CashCare.Models.Wallet
{
    public class Debt
    {
        public DebtType DebtType { get; set; }
        public decimal Amount { get; set; }
    }
}
