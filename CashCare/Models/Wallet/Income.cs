using CashCare.Models.Wallet.Enum;

namespace CashCare.Models.Wallet
{
    public class Income
    {
        public IncomeType TypeOfIncme { get; set; }
        public decimal Amount { get; set; }
    }
}
