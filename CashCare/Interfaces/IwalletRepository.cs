﻿using CashCare.Models.Wallet;

namespace CashCare.Interfaces
{
    public interface IwalletRepository
    {
        public Income GetIncomeById(int id);
    }
}