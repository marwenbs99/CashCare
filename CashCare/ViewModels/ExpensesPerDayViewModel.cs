namespace CashCare.ViewModels
{
    public class ExpensesPerDayViewModel
    {
        public DateTime DayOftheMonth { get; set; }
        public decimal ExpensesTotalAmount { get; set; }
        public decimal SavingThisDay { get; set; }
    }
}
