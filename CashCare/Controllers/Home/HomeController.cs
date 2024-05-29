using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models;
using CashCare.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CashCare.Controllers.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IwalletRepository _iwalletRepository;
        private readonly IDailyExpenseRepository _idailyExpenseRepository;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IwalletRepository iwalletRepository, IDailyExpenseRepository idailyExoenseRepository)
        {
            _logger = logger;
            _context = context;
            _iwalletRepository = iwalletRepository;
            _idailyExpenseRepository = idailyExoenseRepository;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int limit = 0;
            var userInscriptionDate = _context.AppUsers.Where(user => user.Id == userId).Select(user => user.DateOfInscription).FirstOrDefault();

            DailyExpenseViewModel userDailyExpense = new DailyExpenseViewModel();


            userDailyExpense.currentWallet.Wallet = _iwalletRepository.GetCurrentWallet(userId);
            userDailyExpense.todaytotalExpense = _idailyExpenseRepository.GetTotalExpenseToday(userId, DateTime.Now.Day);
            var salaryDateofRecive = userDailyExpense.currentWallet.Wallet.Incomes.FirstOrDefault().DataOfRecive;

            if (userInscriptionDate.Month == DateTime.Now.Month && userInscriptionDate.Year == DateTime.Now.Year) limit = userInscriptionDate.Day;

            DateTime userSalaryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, salaryDateofRecive);


            // Nombre de jours entre 27 actuel et le 27 du mois prochain
            int daysUntilCurrentSalary = (userSalaryDate.AddMonths(1) - userSalaryDate).Days;

            // D�terminer la date de d�but de la liste des d�penses
            DateTime startDate = userSalaryDate;
            if (DateTime.Now.Day < salaryDateofRecive)
            {
                // Si nous sommes avant le 27 du mois, nous devons afficher les d�penses du mois pr�c�dent
                startDate = startDate.AddMonths(-1);
            }

            for (DateTime date = DateTime.Now; date >= startDate; date = date.AddDays(-1))
            {
                var expenseForThisDay = _idailyExpenseRepository.GetTotalExpenseToday(userId, date.Day);
                userDailyExpense.ListexpensePerDay.Add(new ExpensesPerDayViewModel
                {
                    DayOftheMonth = date.Day,
                    ExpensesTotalAmount = expenseForThisDay,
                    SavingThisDay = (userDailyExpense.currentWallet.NettIncomeAfter / daysUntilCurrentSalary) - expenseForThisDay
                });
            }

            //for (int i = DateTime.Now.Day; i >= limit; i--)
            //{
            //    var expenseforthisday = _idailyExpenseRepository.GetTotalExpenseToday(userId, i);
            //    userDailyExpense.ListexpensePerDay.Add(new ExpensesPerDayViewModel
            //    {
            //        DayOftheMonth = i,
            //        ExpensesTotalAmount = expenseforthisday,
            //        SavingThisDay = (userDailyExpense.currentWallet.NettIncomeAfter / 30) - expenseforthisday
            //    });
            //}

            return View(userDailyExpense);
        }

        public IActionResult DayDetails(int dateOfTheMonth)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var listOfexpense = _idailyExpenseRepository.GetListofExpenseThisDay(userId, dateOfTheMonth);

            return View(listOfexpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDailyExpense(IList<DailyExpense> currentList)
        {
            var dayOfTheMonth = 0;
            foreach (var expense in currentList)
            {
                if (expense.Amount != 0)
                {
                    expense.AppUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    dayOfTheMonth = expense.Date.Day;
                    _context.ExpensesDaily.Update(expense);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("DayDetails", "Home", new { dateOfTheMonth = dayOfTheMonth });
        }


        public IActionResult DeleteOneDailyExpense(int dailyExpenseId)
        {
            var expenseToDelete = _context.ExpensesDaily.FirstOrDefault(ex => ex.Id == dailyExpenseId);
            var dayOfTheMonth = expenseToDelete.Date.Day;

            try
            {
                _context.ExpensesDaily.Remove(expenseToDelete);
            }
            catch (Exception ex) { }

            _context.SaveChanges();
            return RedirectToAction("DayDetails", "Home", new { dateOfTheMonth = dayOfTheMonth });
        }


        public IActionResult Investing()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDailyExpense(DailyExpenseViewModel dailyexpenseVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            dailyexpenseVM.currentExpense.AppUserId = int.Parse(userId);
            _context.ExpensesDaily.Add(dailyexpenseVM.currentExpense);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
