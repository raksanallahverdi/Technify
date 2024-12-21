﻿using Technify.Areas.Admin.Models.User;
using Technify.Constants;
using Technify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using Technify.Data;

namespace Technify.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller

    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

        }

        #region Index
        public IActionResult Index()
        {
            var users = new List<UserVM>();
            foreach (var user in _userManager.Users.ToList())
            {
                if (!_userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    users.Add(new UserVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(user).Result.ToList()
                    });
                }

            }

            var model = new UserIndexVM
            {
                Users = users
            };
            return View(model);
        }
        #endregion


        #region Delete

        [HttpPost]
        public IActionResult Delete(string id)
        {
			
			var user = _userManager.FindByIdAsync(id).Result;
			if (user == null) return NotFound();

			
			var userWorks = _context.Works.Where(w => w.UserId == id).ToList();

			
			_context.Works.RemoveRange(userWorks);

			
			_context.SaveChanges();

			
			var result = _userManager.DeleteAsync(user).Result;
			if (!result.Succeeded) return NotFound();

			return RedirectToAction(nameof(Index));
		}
        #endregion

        #region Detail
        [HttpGet]
        public IActionResult Detail(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null) return NotFound();

            var roles = _userManager.GetRolesAsync(user).Result;


            var model = new UserDetailVM
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return View(model);
        }

        #endregion

        #region Update
        public IActionResult Update(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null) return NotFound();
            List<string> roleIds = new List<string>();
            var userRoles = _userManager.GetRolesAsync(user).Result;
            foreach (var userRole in userRoles)
            {
                var role = _roleManager.FindByNameAsync(userRole).Result;
                roleIds.Add(role.Id);
            }

            var model = new UserUpdateVM
            {

                Email = user.Email


            };
            return View(model);


        }

        [HttpPost]
        public IActionResult Update(string id, UserUpdateVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null) return NotFound();
          
            user.PhoneNumber = model.PhoneNumber;
            if (model.NewPassword is not null)
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
        
            
            var updateResult=_userManager.UpdateAsync(user).Result;
            if (!updateResult.Succeeded)
            {
                foreach(var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion
    }
}
