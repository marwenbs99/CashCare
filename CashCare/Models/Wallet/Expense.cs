﻿using CashCare.Models.Wallet.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCare.Models.Wallet
{
    public class Expense
    {
        public int Id { get; set; }
        public ExpenseType TypeOfExpense { get; set; }
        public decimal Amount { get; set; }
        public string details { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public bool Validate()
        {
            if (TypeOfExpense.ToString() == "")
            {
                return false;
            }

            if (Amount <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
