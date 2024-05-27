using CashCare.Data;
using CashCare.Interfaces;
using CashCare.Models.Wallet;
using CashCare.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CashCare.Controllers.Wallet
{
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IwalletRepository _walletRepository;
        public WalletController(ApplicationDbContext context, IwalletRepository iwalletRepository)
        {
            _context = context;
            _walletRepository = iwalletRepository;
        }
        public IActionResult Index(MenuStateVM? menu)
        {
            var currentWallet = GetCurrentWallet();
            WalletViewModel WalletVM = new WalletViewModel
            {
                Wallet = currentWallet,
                MenuState = menu ?? new MenuStateVM(),
            };

            return View(WalletVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(WalletViewModel walletVM)
        {
            //if (!ModelState.IsValid) return View(walletVM);

            var currentWallet = GetCurrentWallet();

            switch (walletVM.BtnAction)
            {
                case ViewModels.Enum.ButtonActionType.IncomeBTN:

                    if (!walletVM.Income.Validate())
                    {
                        //TODO add notification "User have to verify that all input are correctly added"
                        return RedirectToAction("index");
                    }

                    Income income = new Income
                    {
                        Amount = walletVM.Income.Amount,
                        TypeOfIncome = walletVM.Income.TypeOfIncome,
                        DataOfRecive = walletVM.Income.DataOfRecive,
                        WalletId = currentWallet.Id,
                    };

                    _context.Incomes.Add(income);
                    _context.SaveChanges();

                    MenuStateVM currentMenuStatet = new MenuStateVM
                    {
                        IncomeMenu = "show",
                    };

                    return RedirectToAction("Index", currentMenuStatet);
                    //breack;
            }

            return RedirectToAction("Index");
        }


        public IActionResult DeleteIncome(int id)
        {
            MenuStateVM currentMenuSTatet = new MenuStateVM
            {
                IncomeMenu = "show",
            };
            try
            {
                var incomeToDelete = _walletRepository.GetIncomeById(id);
                if (incomeToDelete != null)
                {
                    _context.Incomes.Remove(incomeToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("Index", currentMenuSTatet);
        }

        private Models.Wallet.Wallet GetCurrentWallet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var currentWallet = _context.Wallets
                                                    .Include(w => w.Debts)   // Inclure les dettes
                                                    .Include(w => w.ExpenseListe)  // Inclure les dépenses
                                                    .Include(w => w.Incomes)  // Inclure les revenus
                                                    .FirstOrDefault(u => u.UserId == int.Parse(userId));

                return currentWallet;
            }
            return null;
        }
    }
}
