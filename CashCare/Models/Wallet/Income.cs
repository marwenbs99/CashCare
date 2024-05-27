using CashCare.Models.Wallet.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCare.Models.Wallet
{
    public class Income
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The type of income is required")]
        public IncomeType TypeOfIncome { get; set; }
        [Required(ErrorMessage = "The amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Data of recive is required")]
        public int DataOfRecive { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public bool Validate()
        {
            if (TypeOfIncome.ToString() == "")
            {
                return false;
            }

            if (Amount <= 0)
            {
                return false;
            }

            if (DataOfRecive == default)
            {
                return false;
            }

            return true;
        }
    }
}
