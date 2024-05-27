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
            MenuStateVM currentMenuStatet = new MenuStateVM();

            switch (walletVM.BtnAction)
            {
                case ViewModels.Enum.ButtonActionType.IncomeBTN:

                    currentMenuStatet = new MenuStateVM { IncomeMenu = "show", DebtMenu = "hide", ExpenseMenu = "hide" };

                    if (!walletVM.Income.Validate())
                    {
                        //TODO add notification "User have to verify that all input are correctly added"
                        return RedirectToAction("index", currentMenuStatet);
                    }

                    Income newIncome = new Income
                    {
                        Amount = walletVM.Income.Amount,
                        TypeOfIncome = walletVM.Income.TypeOfIncome,
                        DataOfRecive = walletVM.Income.DataOfRecive,
                        WalletId = currentWallet.Id,
                    };

                    _context.Incomes.Add(newIncome);
                    _context.SaveChanges();

                    break;

                case ViewModels.Enum.ButtonActionType.DebtBTN:

                    currentMenuStatet = new MenuStateVM { IncomeMenu = "hide", DebtMenu = "show", ExpenseMenu = "hide" };

                    if (!walletVM.Debt.Validate())
                    {
                        //TODO add notification "User have to verify that all input are correctly added"
                        return RedirectToAction("index", currentMenuStatet);
                    }

                    Debt newDebt = new Debt
                    {
                        Amount = walletVM.Debt.Amount,
                        DebtType = walletVM.Debt.DebtType,
                        WalletId = currentWallet.Id,
                    };

                    _context.Debts.Add(newDebt);
                    _context.SaveChanges();

                    break;

                case ViewModels.Enum.ButtonActionType.ExpenseBTN:

                    currentMenuStatet = new MenuStateVM { IncomeMenu = "hide", DebtMenu = "hide", ExpenseMenu = "show" };

                    if (!walletVM.Expense.Validate())
                    {
                        //TODO add notification "User have to verify that all input are correctly added"
                        return RedirectToAction("index", currentMenuStatet);
                    }

                    Expense newExpense = new Expense
                    {
                        Amount = walletVM.Expense.Amount,
                        TypeOfExpense = walletVM.Expense.TypeOfExpense,
                        details = walletVM.Expense.details,
                        WalletId = currentWallet.Id,
                    };

                    _context.Expenses.Add(newExpense);
                    _context.SaveChanges();

                    break;
            }

            return RedirectToAction("Index", currentMenuStatet);
        }


        public IActionResult DeleteIncome(int id)
        {
            MenuStateVM currentMenuSTatet = new MenuStateVM { IncomeMenu = "show", };
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
        public IActionResult EditIncome(int id)
        {
            Income currentIncome = _walletRepository.GetIncomeById(id);
            return View(currentIncome);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditIncome(Income currentIncome)
        {
            if (!currentIncome.Validate())
            {
                return View(currentIncome);
            }

            // Récupérer l'objet Income existant
            var existingIncome = _context.Incomes.Find(currentIncome.Id);

            // Vérifier si l'objet existe
            if (existingIncome != null)
            {
                // Mettre à jour les propriétés de l'objet existant avec les nouvelles valeurs
                existingIncome.TypeOfIncome = currentIncome.TypeOfIncome;
                existingIncome.Amount = currentIncome.Amount;
                existingIncome.DataOfRecive = currentIncome.DataOfRecive;
                _context.SaveChanges();
            }
            else
            {
                // Gérer le cas où l'objet n'existe pas
            }

            MenuStateVM currentMenuSTatet = new MenuStateVM { IncomeMenu = "show", };
            return RedirectToAction("Index", currentMenuSTatet);
        }

        public IActionResult DeleteDebt(int id)
        {
            MenuStateVM currentMenuSTatet = new MenuStateVM { DebtMenu = "show", };
            try
            {
                var debtToDelete = _walletRepository.GetDebtById(id);
                if (debtToDelete != null)
                {
                    _context.Debts.Remove(debtToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("Index", currentMenuSTatet);
        }

        public IActionResult EditDebt(int id)
        {
            Debt currentDebt = _walletRepository.GetDebtById(id);
            return View(currentDebt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDebt(Debt currentDebt)
        {
            if (!currentDebt.Validate())
            {
                return View(currentDebt);
            }

            // Récupérer l'objet Income existant
            var existingDebt = _context.Debts.Find(currentDebt.Id);

            // Vérifier si l'objet existe
            if (existingDebt != null)
            {
                // Mettre à jour les propriétés de l'objet existant avec les nouvelles valeurs
                existingDebt.DebtType = currentDebt.DebtType;
                existingDebt.Amount = currentDebt.Amount;
                _context.SaveChanges();
            }
            else
            {
                // Gérer le cas où l'objet n'existe pas
            }

            MenuStateVM currentMenuSTatet = new MenuStateVM { DebtMenu = "show", };
            return RedirectToAction("Index", currentMenuSTatet);
        }

        public IActionResult DeleteExpense(int id)
        {
            MenuStateVM currentMenuSTatet = new MenuStateVM { ExpenseMenu = "show", };
            try
            {
                var expenseToDelete = _walletRepository.GetExpenseById(id);
                if (expenseToDelete != null)
                {
                    _context.Expenses.Remove(expenseToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("Index", currentMenuSTatet);
        }

        public IActionResult EditExpense(int id)
        {
            Expense currentExpense = _walletRepository.GetExpenseById(id);
            return View(currentExpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditExpense(Expense currentExpense)
        {
            if (!currentExpense.Validate())
            {
                return View(currentExpense);
            }

            // Récupérer l'objet Income existant
            var existingExpense = _context.Expenses.Find(currentExpense.Id);

            // Vérifier si l'objet existe
            if (existingExpense != null)
            {
                // Mettre à jour les propriétés de l'objet existant avec les nouvelles valeurs
                existingExpense.TypeOfExpense = currentExpense.TypeOfExpense;
                existingExpense.Amount = currentExpense.Amount;
                existingExpense.details = currentExpense.details;
                _context.SaveChanges();
            }
            else
            {
                // Gérer le cas où l'objet n'existe pas
            }

            MenuStateVM currentMenuSTatet = new MenuStateVM { ExpenseMenu = "show", };
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
