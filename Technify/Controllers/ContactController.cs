using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technify.Data;
using Technify.Entities;

namespace Technify.Controllers;
[Authorize]

public class ContactController : Controller
{
    private readonly AppDbContext _context;

    public ContactController(AppDbContext context)
    {
        _context = context;
    }
	public IActionResult Index()
	{
		return View();
	}
	[HttpPost]
    public IActionResult SendMessage(MessageVM messageVM)
    {
        if (ModelState.IsValid)
        {
            // Map MessageVM to ContactMessage
            var contactMessage = new ContactMessage
            {
                Name = messageVM.Name,
                Email = messageVM.Email,
                Subject = messageVM.Subject,
                Message = messageVM.Message
            };

           
            _context.ContactMessages.Add(contactMessage);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Index", "Home");
        }

        TempData["ErrorMessage"] = "There was an error. Please correct the highlighted fields.";

        return View("Index", messageVM);
    }
}

