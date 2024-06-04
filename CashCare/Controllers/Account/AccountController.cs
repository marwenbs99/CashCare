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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);

                currentUser.PhoneNumber = MasquerNumero(currentUser.PhoneNumber);

                return View(new EditAccountVM { CurrentUser = currentUser });
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult EditMail()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);
            EditMailViewModel vm = new EditMailViewModel();
            vm.Email = currentUser.Email;
            return View(vm);
        }

        [HttpPost]
        [ActionName("EditEmail")]
        public ActionResult EditMailPost(EditMailViewModel newMail)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);
                currentUser.Email = newMail.Email;
                _context.AppUsers.Update(currentUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("EditAccount");
        }

        public ActionResult EditPhone()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);
            EditPhoneVM vm = new EditPhoneVM();
            vm.PhoneNumber = MasquerNumero(currentUser.PhoneNumber);
            return View(vm);
        }

        [HttpPost]
        [ActionName("EditPhone")]
        public ActionResult EditPhonePost(EditPhoneVM currentPhone)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);
                currentUser.PhoneNumber = currentPhone.PhoneNumber;
                _context.AppUsers.Update(currentUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("EditAccount");
        }

        public ActionResult EditPassword()
        {
            return View(new EditPasswordVM());
        }

        [HttpPost]
        [ActionName("EditPassword")]
        public ActionResult EditPasswordPost(EditPasswordVM editPasswordVM)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);
            var passwordHasher = new PasswordHasher<AppUser>();
            var verificationResult = passwordHasher.VerifyHashedPassword(currentUser, currentUser.Password, editPasswordVM.CurrentPassword);


            if (!ModelState.IsValid || !editPasswordVM.NewPassword.Equals(editPasswordVM.ConfirmPassword) || verificationResult != PasswordVerificationResult.Success)
            {
                return View();
            }

            currentUser.Password = passwordHasher.HashPassword(currentUser, editPasswordVM.NewPassword);
            _context.AppUsers.Update(currentUser);
            _context.SaveChanges();

            return RedirectToAction("EditAccount");
        }

        public ActionResult EditUserName()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);

            return View(new EditUserNameVM { FirstName = currentUser.FirstName, LastName = currentUser.LastName });
        }

        [HttpPost]
        [ActionName("EditUserName")]
        public ActionResult EditUserNamePost(EditUserNameVM editUserNameVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editUserNameVM);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            AppUser currentUser = _context.AppUsers.FirstOrDefault(user => user.Id == userId);

            currentUser.FirstName = editUserNameVM.FirstName;
            currentUser.LastName = editUserNameVM.LastName;
            _context.AppUsers.Update(currentUser); _context.SaveChanges();

            return RedirectToAction("EditAccount");
        }

        public static string MasquerNumero(string numero)
        {
            // Vérifiez que le numéro a une longueur correcte
            if (numero.Length < 6)
            {
                return "";
            }

            // Gardez les trois premiers caractères et les quatre derniers
            string debut = numero.Substring(0, 3);
            string fin = numero.Substring(numero.Length - 4);

            // Remplacez les caractères intermédiaires par des astérisques
            string masque = new string('*', numero.Length - 7);

            // Combinez les parties pour obtenir le numéro masqué
            string numeroMasque = debut + masque + fin;

            return numeroMasque;
        }
    }
}
