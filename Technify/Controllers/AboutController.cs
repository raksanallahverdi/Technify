using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Technify.Controllers;
[Authorize]

public class AboutController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
