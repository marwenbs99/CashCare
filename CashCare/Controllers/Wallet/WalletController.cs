using Microsoft.AspNetCore.Mvc;

namespace CashCare.Controllers.Wallet
{
    public class WalletController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
