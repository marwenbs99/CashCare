using CashCare.Models;
using CashCare.Models.Wallet;
using Microsoft.EntityFrameworkCore;

namespace CashCare.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Debt> Debts { get; set; }
    }
}
