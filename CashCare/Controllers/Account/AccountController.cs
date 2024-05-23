using CashCare.Data;
using CashCare.Models;
using CashCare.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CashCare.Controllers.Account
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginInfo)
        {
            var currentUser = _context.AppUsers.FirstOrDefault(user => user.Email.Equals(loginInfo.Email) && user.Password.Equals(loginInfo.Password));

            if (currentUser != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignUpViewModel SignupVM)
        {
            if (SignupVM.Password != SignupVM.RepeatedPassword)
            {
                return View();
            }

            AppUser newUser = new AppUser
            {
                Email = SignupVM.Email,
                Password = SignupVM.Password,
                FirstName = SignupVM.FirstName,
                LastName = SignupVM.LastName,
                PhoneNumber = SignupVM.PhoneNumber,
            };

            _context.AppUsers.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
    }
}
