﻿using CashCare.Models.Wallet;
using CashCare.ViewModels.Enum;

namespace CashCare.ViewModels
{
    public class WalletViewModel
    {
        public Models.Wallet.Wallet Wallet { get; set; } = new Models.Wallet.Wallet();
        public ButtonActionType BtnAction { get; set; }
        public Income Income { get; set; }

        public MenuStateVM MenuState { get; set; }

        public decimal TotalIncome
        {
            get
            {
                return Wallet?.Incomes?.Sum(i => i.Amount) ?? 0;
            }
        }



    }
}