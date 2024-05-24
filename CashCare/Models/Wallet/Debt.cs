using CashCare.Models.Wallet.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCare.Models.Wallet
{
    public class Debt
    {
        public int Id { get; set; }
        public DebtType DebtType { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
