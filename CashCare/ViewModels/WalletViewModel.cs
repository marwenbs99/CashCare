using CashCare.Models.Wallet;
using CashCare.ViewModels.Enum;

namespace CashCare.ViewModels
{
    public class WalletViewModel
    {
        public Models.Wallet.Wallet Wallet { get; set; }
        public ButtonActionType BtnAction { get; set; }
        public Income Income { get; set; }

    }
}
