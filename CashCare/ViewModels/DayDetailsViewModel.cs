using CashCare.Models;

namespace CashCare.ViewModels
{
    public class DayDetailsViewModel
    {
        public IList<DailyExpense> ListDailyExpense { get; set; } = new List<DailyExpense>();
        public List<DailyExpense> NewDailyExpense { get; set; } = new List<DailyExpense>();

        public DateTime Date { get; set; }
    }
}
