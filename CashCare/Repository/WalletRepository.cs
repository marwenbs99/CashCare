﻿using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models.Wallet;

namespace CashCare.Repository
{
    public class WalletRepository : IwalletRepository
    {
        private readonly ApplicationDbContext _context;
        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Income GetIncomeById(int id)
        {
            var currentIncome = _context.Incomes.FirstOrDefault(x => x.Id == id);

            return currentIncome;
        }
    }
}
