using LinkDev.IKEA.BLL.Services.EmailService;
using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.PL.Helper;
using LinkDev.IKEA.PL.Models.Identity;
using LinkDev.IKEA.PL.Models.SMS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace LinkDev.IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ISMSService _sMSService;

        //private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IMailService mailService,
                                ISMSService sMSService,
                                IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _sMSService = sMSService;
            //_emailSender = emailSender;
        }
        #region Sign_Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "UserName is already taken");
                return View(model);
            }


            user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                IsAgree = model.IsAgree,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(SignIn));


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);


        }
        #endregion

        #region Sign-In
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

                    if (result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "You are not allowed to sign in.");

                    if (result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public IActionResult GoogleLogIn()
        {
            var URI = Url.Action("GoogleResponse", "Account");
            var prop = _signInManager.ConfigureExternalAuthenticationProperties(
                GoogleDefaults.AuthenticationScheme, URI
            );
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);

        }


        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
                return RedirectToAction(nameof(SignIn));
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false
            );
            if(signInResult.Succeeded)
                return RedirectToAction("Index", "Home");
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if(email == null)
                return RedirectToAction(nameof(SignIn));

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "NA",
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "NA",
                    IsAgree = true,
                    EmailConfirmed = true
                };

                var CreateResult =  await _userManager.CreateAsync(user);
                if(!CreateResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error creating user account.");
                    return RedirectToAction(nameof(SignIn));
                }
            }
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        #endregion


        #region Sign_Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Reset_Password
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendRestPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ChangePassword", "Account", new { email = forgetPasswordViewModel.Email, token }, Request.Scheme);
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Password",
                        Body = $"Please reset your password by clicking <a href='{url}'>here</a>."
                    };
                    //_emailSender.SendEmail(email);
                    _mailService.Send(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid operation please try again.");
            }
            return View(forgetPasswordViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SendRestPasswordUrlSMS(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ChangePassword", "Account", new { email = forgetPasswordViewModel.Email, token }, Request.Scheme) ?? string.Empty;

                    var sms = new SMSMessage()
                    {
                        PhoneNumber = user.PhoneNumber ?? string.Empty,
                        Body = url 
                    };
                    _sMSService.SendSMS(sms);
                    return Ok("Check Your SMS");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid operation please try again.");
            }
            return View(nameof(ResetPassword),forgetPasswordViewModel);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset_Password
        [HttpGet]
        public IActionResult ChangePassword(string email, string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid password reset token.");
            }

            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var email = TempData["email"] as string;
            var token = TempData["token"] as string;
            if (email == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid password reset token.");
                return View(model);
            }
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        #endregion


    }
}
