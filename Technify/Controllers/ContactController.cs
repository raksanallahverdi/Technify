using Microsoft.AspNetCore.Mvc;

namespace Technify.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
