using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolNetwork.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SchoolNetwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public List<AuthenticationScheme> ExternalLogins { get; private set; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return Redirect("/Home/Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = "",
                };
                var regsiterResult = await _userManager.CreateAsync(user, password);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                if (regsiterResult.Succeeded)
                {
                    var callbackUrl = Url.Action(
                        action: nameof(ConfirmEmail),
                        controller: "Account",
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme,
                        host: Request.Host.ToString());

                    var emailSender = HtmlEncoder.Default.Encode(callbackUrl);
                    TempData["_emailSender"] = callbackUrl;

                    var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    return RedirectToAction("EmailConfirmation");
                }
            }
            return RedirectToAction("Register");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailConfirmation()
        {
            var email = TempData["_emailSender"];
            ViewBag.Email = email;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}