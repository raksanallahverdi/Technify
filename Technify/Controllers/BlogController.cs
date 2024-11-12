using Microsoft.AspNetCore.Mvc;

namespace Technify.Controllers
{
	public class BlogController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
