using TaskTracker.Models;
using TaskTracker.Models.Auth;
using TaskTracker.Repo.Interfaces;
using TaskTracker.UI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TaskTracker.Models.CommonModel;

namespace TaskTracker.UI.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly IAccountRepo _repo;
        private readonly IMenuAuthorizationRepo _menuRepo;
        public AccountController(IAccountRepo repo, IMenuAuthorizationRepo menuRepo)
        {
            _repo = repo;
            _menuRepo = menuRepo;
        }
        //public async Task<IActionResult> _leftSideBar()
        //{
        //    try
        //    {
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var token= HttpContext.Session.GetString("Token");
        //        var result = await _menuRepo.GetAssignedMenuListAsync(userId, token);
        //        var model = result.Data as List<UserMenu> ?? new List<UserMenu>();
        //        ViewBag.ShouldRenderMenu = model.Any();
        //        return PartialView("_leftSideBar", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        return PartialView("_leftSideBar", Enumerable.Empty<UserMenu>());
        //    }
        //}
        [AllowAnonymous]

        public async Task<IActionResult> _leftSideBar()
        {
            try
            {
                var cachedMenuJson = HttpContext.Session.GetString("UserMenu");

                List<UserMenu> model;

                if (!string.IsNullOrEmpty(cachedMenuJson))
                {
                    model = System.Text.Json.JsonSerializer.Deserialize<List<UserMenu>>(cachedMenuJson);
                }
                else
                {
                    var userId = HttpContext.Session.GetString("UserId");
                    var token = HttpContext.Session.GetString("Token");

                    var result = await _menuRepo.GetAssignedMenuListAsync(userId, token);
                    model = result.Data as List<UserMenu> ?? new List<UserMenu>();

                    HttpContext.Session.SetString("UserMenu", System.Text.Json.JsonSerializer.Serialize(model));
                }

                ViewBag.ShouldRenderMenu = model.Any();
                return PartialView("_leftSideBar", model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LEFT SIDEBAR ERROR] {ex.Message}");
                return PartialView("_leftSideBar", Enumerable.Empty<UserMenu>());
            }
        }

        public IActionResult AccessDenied()
        {
            var role = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = role;
            return View();
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            var role = HttpContext.Session.GetString("Role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role.ToLower() != "customer")
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields correctly.";
                return View(vm);
            }

            var result = await _repo.LoginAsync(vm);

            if (result.Status != "Success" || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Login failed.";
                return View(vm);
            }

            var tokens = JsonConvert.DeserializeObject<AuthModel>(result.Data.ToString());
            var token = tokens?.token;

            var jwtInfo = JwtHelper.DecodeJwt(token);

            if (jwtInfo == null)
            {
                TempData["Error"] = "Invalid token received.";
                return View(vm);
            }

            SessionHelper.SetUserSession(HttpContext, jwtInfo, token);

            if (jwtInfo.Role?.ToLower() != "customer")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _repo.RegisterAsync(vm);

            if (result.Status == "Success")
            {
                TempData["Success"] = "Registration successful! Please check your email.";
                TempData["RegisteredEmail"] = vm.Email;
                return RedirectToAction("CheckEmail");
            }

            TempData["Error"] = result.Message ?? "Registration failed.";
            return View(vm);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult CheckEmail()
        {
            var email = TempData["RegisteredEmail"] as string;
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<JsonResult> ResendVerification(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { status = "Fail", message = "Email is required." });

            var result = await _repo.ResendVerificationAsync(email); 

            if (result.Status?.ToLower() == "success")
            {
                return Json(new { status = "Success", message = "Verification email sent successfully. Please check your inbox." });
            }
            return Json(new { status = "Fail", message = result.Message ?? "Failed to resend verification email." });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Session.GetString("Token");

            var result = await _repo.LogoutAsync(token);
            HttpContext.Session.Clear();

            HttpContext.Session.Clear();
            foreach (var cookieKey in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookieKey);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _repo.ForgetPasswordAsync(vm);

            if (result.Status != "Success")
            {
                ModelState.AddModelError("", result.Message ?? "Unable to send reset link.");
                return View(vm);
            }

            TempData["SuccessMessage"] = "We’ve sent a password reset link to your email!";
            return RedirectToAction("ForgetPassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _repo.ResetPasswordAsync(vm);

            if (result.Status != "Success")
            {
                ModelState.AddModelError("", result.Message ?? "Password reset failed.");
                return View(vm);
            }

            TempData["SuccessMessage"] = "Password reset successfully! Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.UserId) || string.IsNullOrEmpty(vm.Token))
                return RedirectToAction("Login");

            var result = await _repo.VerifyEmailAsync(vm);

            if (result.Status?.ToLower() == "success")
            {
                ViewBag.IsSuccess = true;
                ViewBag.Message = "Your email has been verified successfully! You can now login.";
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = result.Message ?? "Email verification failed or the link has expired.";
            }

            return View("VerifyEmail");
        }

    }
}
