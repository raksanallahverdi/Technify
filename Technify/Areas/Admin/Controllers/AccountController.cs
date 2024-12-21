﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Technify.Entities;
using Technify.Areas.Admin.Models.Account;
using Microsoft.EntityFrameworkCore;
using Technify.Utilities.EmailHandler.Abstract;
using Technify.Utilities.EmailHandler.Models;

namespace Technify.Areas.Admin.Controllers;

[Area("admin")]
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
   [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(AccountLoginVM model)
        {
        if (!ModelState.IsValid) return View(model);
        var user=_userManager.FindByEmailAsync(model.Email).Result;
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);

        }
        var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
        if (!isAdmin) {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }

        var result=_signInManager.PasswordSignInAsync(user, model.Password,false,false).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }
        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }
       
        return RedirectToAction("index","dashboard");           
    }

   
}
