using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolNetwork.Data;
using SchoolNetwork.Models;
using SchoolNetwork.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SchoolNetwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public List<AuthenticationScheme> ExternalLogins { get; private set; }

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
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
        public async Task<IActionResult> Register(ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstMidName = model.FirstMidName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    JoinDate = System.DateTime.Now,
                };

                var regsiterResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role.ToString());

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                if (regsiterResult.Succeeded && roleResult.Succeeded)
                {
                    var callbackUrl = Url.Action(
                        action: nameof(ConfirmEmail),
                        controller: "Account",
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme,
                        host: Request.Host.ToString());

                    var emailSender = HtmlEncoder.Default.Encode(callbackUrl);
                    TempData["_emailSender"] = callbackUrl;

                    var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("EmailConfirmation");
                }
            }
            return View(model);
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
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var result = await _signInManager.PasswordSignInAsync(username, password, true, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Results()
        {
            var user = await GetCurrentUserId();

            var results = await _context.Results.Where(c => c.ApplicationUserID == user)
                .OrderByDescending(m => m.ResultDate)
                .Include(a => a.Assignment)
                    .ThenInclude(a => a.Course)
                .AsNoTracking()
                .ToListAsync();

            var userResultViewModel = new UserResultViewModel();

            foreach (var r in results)
            {
                userResultViewModel.Results.Add(r);
                userResultViewModel.Assignments.Add(r.Assignment);
            }

            return View(userResultViewModel);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<int> GetCurrentUserId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr.Id;
        }
    }
}