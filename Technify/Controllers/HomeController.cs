﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Technify.Controllers;

[Authorize]
public class HomeController : Controller
{

	public IActionResult Index()
	{
		return View();
	}
}
