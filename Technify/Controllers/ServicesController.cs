using Microsoft.AspNetCore.Mvc;

namespace Technify.Controllers
{
	public class ServicesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
