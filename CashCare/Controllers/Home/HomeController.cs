using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models;
using CashCare.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
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
            userDailyExpense.todaytotalExpense = _idailyExpenseRepository.GetTotalExpenseToday(userId, DateTime.Now);
            var salaryDateofRecive = userDailyExpense.currentWallet.Wallet.Incomes.FirstOrDefault()?.DataOfRecive;

            if (salaryDateofRecive != null)
            {

                if (userInscriptionDate.Month == DateTime.Now.Month && userInscriptionDate.Year == DateTime.Now.Year) limit = userInscriptionDate.Day;

                DateTime userSalaryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, salaryDateofRecive ?? 0);


                // Nombre de jours entre 27 actuel et le 27 du mois prochain
                int daysUntilCurrentSalary = (userSalaryDate.AddMonths(1) - userSalaryDate).Days;

                // Déterminer la date de début de la liste des dépenses
                DateTime startDate = userSalaryDate;
                if (DateTime.Now.Day < salaryDateofRecive)
                {
                    // Si nous sommes avant le 27 du mois, nous devons afficher les dépenses du mois précédent
                    startDate = startDate.AddMonths(-1);
                }

                for (DateTime date = DateTime.Now; date >= startDate; date = date.AddDays(-1))
                {
                    var expenseForThisDay = _idailyExpenseRepository.GetTotalExpenseToday(userId, date);
                    userDailyExpense.ListexpensePerDay.Add(new ExpensesPerDayViewModel
                    {
                        DayOftheMonth = date,
                        ExpensesTotalAmount = expenseForThisDay,
                        SavingThisDay = (userDailyExpense.currentWallet.NettIncomeAfter / daysUntilCurrentSalary) - expenseForThisDay
                    });
                }
            }
            var currentCulture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            ViewData["SelectedCulture"] = currentCulture;

            return View(userDailyExpense);
        }

        public IActionResult DayDetails(DateTime dateOfTheMonth)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            DayDetailsViewModel dayDetailsViewModel = new DayDetailsViewModel();
            dayDetailsViewModel.ListDailyExpense = _idailyExpenseRepository.GetListofExpenseThisDay(userId, dateOfTheMonth);
            dayDetailsViewModel.Date = dateOfTheMonth;

            return View(dayDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDailyExpense(DayDetailsViewModel dayDetailsViewModel)
        {
            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            foreach (var expense in dayDetailsViewModel.ListDailyExpense)
            {
                if (expense.Amount != 0)
                {
                    expense.AppUserId = UserId;
                    _context.ExpensesDaily.Update(expense);
                }

            }

            if (dayDetailsViewModel.NewDailyExpense.Count() > 0)
            {
                foreach (var newData in dayDetailsViewModel.NewDailyExpense)
                {
                    newData.AppUserId = UserId;
                    newData.Date = dayDetailsViewModel.Date;
                    _context.ExpensesDaily.Add(newData);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("DayDetails", "Home", new { dateOfTheMonth = dayDetailsViewModel.Date });
        }


        public IActionResult DeleteOneDailyExpense(int dailyExpenseId)
        {
            var expenseToDelete = _context.ExpensesDaily.FirstOrDefault(ex => ex.Id == dailyExpenseId);
            var dayOfTheMonth = expenseToDelete?.Date;

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
            try
            {
                dailyexpenseVM.currentExpense.AppUserId = int.Parse(userId);
                _context.ExpensesDaily.Add(dailyexpenseVM.currentExpense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");

        }

        public IActionResult Unsupported()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            ViewData["SelectedCulture"] = culture;

            return LocalRedirect(returnUrl);
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
