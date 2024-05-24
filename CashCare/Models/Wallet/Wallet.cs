using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCare.Models.Wallet
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int UserId { get; set; }
        public IList<Expense> ExpenseListe { get; set; } = new List<Expense>();
        public IList<Debt> Debts { get; set; } = new List<Debt>();
        public IList<Income> Incomes { get; set; } = new List<Income>();
    }
}
