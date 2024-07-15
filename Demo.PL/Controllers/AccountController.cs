using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                // Check if UserName Exists
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    // Check if Email Exists
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };
                        var Result = await _userManager.CreateAsync(user, model.Password);
                        if (Result.Succeeded)
                            return RedirectToAction(nameof(SignIn));
                        foreach (var err in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);
                            return View(model);
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "User Is Already Exist");
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    bool Flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (Flag)
                    {
                        var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (Result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password ):");
            }
            return View(model);
        }

        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate User RestToken
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //GEnerate Reset Url
                    var url = Url.Action(nameof(ResetPassword), "Account", new { email = model.Email, Token = token }, Request.Scheme);

                    // Initialize Email 
                    Email email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipient = model.Email,
                        Body = url
                    };

                    //Sending The Email
                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invaid Email!");
                #region Com
                //var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user is not null)
                //{
                //    // Generate UserToken
                //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                //    // Generate URL 
                //    var url = Url.Action(nameof(ResetPassword), "Account", new { email = model.Email, Token = token }, Request.Scheme);

                //    //Create The Email
                //    var Email = new Email()
                //    {
                //        Subject = "Reset ur Password",
                //        Recipient = model.Email,
                //        Body = url
                //    };

                //    // Send Email
                //    EmailSettings.SendEmail(Email);

                //    return RedirectToAction(nameof(CheckYourInbox));
                //}
                //ModelState.AddModelError(string.Empty, "Email is Invalid!");
                #endregion
            }
            return View(nameof(ForgetPassword), model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewData["email"] = email;
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(model);
        }

        #endregion

    }
}
