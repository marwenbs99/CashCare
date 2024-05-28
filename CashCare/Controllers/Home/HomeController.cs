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
        private readonly IDailyExpenseRepository _idailyExoenseRepository;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IwalletRepository iwalletRepository, IDailyExpenseRepository idailyExoenseRepository)
        {
            _logger = logger;
            _context = context;
            _iwalletRepository = iwalletRepository;
            _idailyExoenseRepository = idailyExoenseRepository;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            DailyExpenseViewModel userDailyExpense = new DailyExpenseViewModel();

            userDailyExpense.currentWallet.Wallet = _iwalletRepository.GetCurrentWallet(userId);
            userDailyExpense.todaytotalExpense = _idailyExoenseRepository.GetTotalExpenseToday(userId, DateTime.Now.Day);

            for (int i = DateTime.Now.Day; i >= 0; i--)
            {
                var expenseforthisday = _idailyExoenseRepository.GetTotalExpenseToday(userId, i);
                userDailyExpense.ListexpensePerDay.Add(new ExpensesPerDayViewModel
                {
                    DayOftheMonth = i,
                    ExpensesTotalAmount = expenseforthisday,
                    SavingThisDay = (userDailyExpense.currentWallet.NettIncomeAfter / 30) - expenseforthisday
                });
            }

            return View(userDailyExpense);
        }

        public IActionResult WeekDetails()
        {
            return View();
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
