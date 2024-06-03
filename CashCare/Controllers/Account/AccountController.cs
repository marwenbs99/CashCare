using CashCare.Data;
using CashCare.Models;
using CashCare.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CashCare.Controllers.Account
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            var currentUser = _context.AppUsers.FirstOrDefault(user => user.Email.Equals(loginInfo.Email));

            if (currentUser != null)
            {
                var passwordHasher = new PasswordHasher<AppUser>();
                var verificationResult = passwordHasher.VerifyHashedPassword(currentUser, currentUser.Password, loginInfo.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, currentUser.Email),
                    new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString())
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignUpViewModel SignupVM)
        {
            if (SignupVM.Password != SignupVM.RepeatedPassword || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid logout attempt.");
                return View(SignupVM);
            }

            var passwordHasher = new PasswordHasher<AppUser>();

            AppUser newUser = new()
            {
                Email = SignupVM.Email,
                FirstName = SignupVM.FirstName,
                LastName = SignupVM.LastName,
                PhoneNumber = SignupVM.PhoneNumber,
                Password = SignupVM.Password,
            };

            newUser.Password = passwordHasher.HashPassword(newUser, SignupVM.Password);

            _context.AppUsers.Add(newUser);
            _context.SaveChanges();

            CashCare.Models.Wallet.Wallet newWallet = new CashCare.Models.Wallet.Wallet
            {
                UserId = newUser.Id,
            };

            _context.Wallets.Add(newWallet);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
        public ActionResult EditAccount()
        {
            return View();
        }
    }
}
