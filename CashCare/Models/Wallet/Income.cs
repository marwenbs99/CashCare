using CashCare.Models.Wallet.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCare.Models.Wallet
{
    public class Income
    {
        public int Id { get; set; }
        public IncomeType TypeOfIncome { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
