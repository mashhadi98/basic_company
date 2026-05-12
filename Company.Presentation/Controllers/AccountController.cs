using System.Text;
using System.Text.Encodings.Web;
using Company.Infrastructure.Persistence.Identity;
using Company.Presentation.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Company.Presentation.Controllers;

/// <summary>
/// احراز هویت مبتنی بر Identity (ثبت‌نام، ورود، خروج، فراموشی رمز، تأیید ایمیل).
/// </summary>
public sealed class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingUsername = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
        if (existingUsername is not null)
        {
            ModelState.AddModelError(nameof(RegisterViewModel.Username), "این نام کاربری قبلاً استفاده شده است.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
        var confirmationLink = Url.Action(
            nameof(ConfirmEmail),
            "Account",
            new { userId = user.Id, token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)) },
            Request.Scheme) ?? string.Empty;

        await _emailSender.SendEmailAsync(
            model.Email,
            "تأیید ایمیل",
            $"برای تأیید حساب روی لینک کلیک کنید: <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>تأیید</a>")
            .ConfigureAwait(false);

        _logger.LogInformation("User registered: {Email}", model.Email);
        return RedirectToAction(nameof(RegisterConfirmation));
    }

    [HttpGet]
    public IActionResult RegisterConfirmation() => View();

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId is null || token is null)
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decoded).ConfigureAwait(false);
        return View(result.Succeeded);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "ورود ناموفق بود.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true).ConfigureAwait(false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "حساب موقتاً قفل شده است.");
            return View(model);
        }

        if (result.IsNotAllowed)
        {
            ModelState.AddModelError(string.Empty, "ابتدا ایمیل خود را تأیید کنید.");
            return View(model);
        }

        ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور نادرست است.");
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
        if (user is null || !(await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)))
        {
            // جلوگیری از Enumeration: همیشه پیام موفق
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
        var callbackUrl = Url.Action(
            nameof(ResetPassword),
            "Account",
            new
            {
                email = user.Email,
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)),
            },
            Request.Scheme) ?? string.Empty;

        await _emailSender.SendEmailAsync(
            user.Email!,
            "بازیابی رمز عبور",
            $"برای تنظیم رمز جدید کلیک کنید: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>بازیابی</a>")
            .ConfigureAwait(false);

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation() => View();

    [HttpGet]
    public IActionResult ResetPassword(string? email, string? token)
    {
        if (email is null || token is null)
        {
            return BadRequest();
        }

        return View(new ResetPasswordViewModel { Email = email, Token = token });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
        if (user is null)
        {
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ResetPasswordAsync(user, decoded, model.Password).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        return RedirectToAction(nameof(ResetPasswordConfirmation));
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation() => View();

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
