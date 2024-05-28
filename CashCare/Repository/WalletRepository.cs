using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models.Wallet;
using Microsoft.EntityFrameworkCore;

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

        public Debt GetDebtById(int id)
        {
            var currentDebt = _context.Debts.FirstOrDefault(x => x.Id == id);
            return currentDebt;
        }

        public Expense GetExpenseById(int id)
        {
            var currentExpense = _context.Expenses.FirstOrDefault(x => x.Id == id);
            return currentExpense;
        }

        public Wallet GetCurrentWallet(int id)
        {
            var currentWallet = _context.Wallets
                                                .Include(w => w.Debts)   // Inclure les dettes
                                                .Include(w => w.ExpenseListe)  // Inclure les dépenses
                                                .Include(w => w.Incomes)  // Inclure les revenus
                                                .FirstOrDefault(u => u.UserId == id);

            return currentWallet;

        }
    }
}
