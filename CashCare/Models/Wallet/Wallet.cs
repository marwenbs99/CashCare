namespace CashCare.Models.Wallet
{
    public class Wallet
    {
        public int Id { get; set; }
        public IList<Expense> ExpenseListe { get; set; } = new List<Expense>();
        public IList<Debt> debts { get; set; } = new List<Debt>();
        public IList<Income> incomes { get; set; } = new List<Income>();
    }
}
