﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Technify.Utilities.EmailHandler.Abstract;
using Technify.Utilities.EmailHandler.Models;
using Microsoft.Extensions.Configuration;
using Technify.Entities;
using Technify.Models.Account;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;
    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;


    }
    #region Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(AccountRegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            Email = model.Email,
            UserName = model.Email,
            PhoneNumber = model.PhoneNumber
        };
        var result = _userManager.CreateAsync(user, model.Password).Result;
        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
                ModelState.AddModelError(string.Empty, item.Description);
            return View(model);

        }
        var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        var url = Url.Action(nameof(ConfirmEmail), "Account", new { token, user.Email }, Request.Scheme);
        _emailService.SendMessage(new Message(new List<string> { user.Email }, "Raksana Email Confirmation", url));

        return RedirectToAction(nameof(Login));
    }
    public IActionResult ConfirmEmail(string email, string token)
    {
        var user = _userManager.FindByEmailAsync(email).Result;
        if (user is null) return NotFound();
        var result = _userManager.ConfirmEmailAsync(user, token).Result;
        if (!result.Succeeded) return NotFound();
        return RedirectToAction(nameof(Login));
    }
    #endregion

    #region Login 

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(AccountLoginVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = _userManager.FindByEmailAsync(model.Email).Result;
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);

        }
        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError(string.Empty, "Your email is not confirmed. Please check your inbox for the confirmation email.");
            return View(model);
        }
        var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }
        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("index", "home");
    }
    #endregion

    #region ForgetPassword

    [HttpGet]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ForgetPassword(AccountForgetPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = _userManager.FindByNameAsync(model.Email).Result;
        if (user is null)
        {
            ModelState.AddModelError("Email", "User Not found");
            return View(model);
        }
        var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
        var url = Url.Action(nameof(ResetPassword), "Account", new { token, user.Email }, Request.Scheme);
        _emailService.SendMessage(new Message(new List<string> { user.Email }, "Raksana Forget Password?", url));
        ViewBag.NotificationText = "Reset Password Mail send successfully";
        return View("Notification");

    }


    #endregion

    #region ResetPassword
    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ResetPassword(AccountResetPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = _userManager.FindByNameAsync(model.Email).Result;
        if (user is null)
        {
            ModelState.AddModelError("Email", "Error while reset password");
            return View(model);
        }
        var result = _userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(model);
        }
        return RedirectToAction(nameof(Login));
    }
    #endregion

    #region Logout
    public async Task<IActionResult> Logout()
    {
        _ = _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));

    }
    #endregion

  
}
