using Technify.Models;
using Technify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technify.Data;
using Technify.Areas.Admin.Models.Contact;

namespace Technify.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        #region Index
        public IActionResult Index()
        {
            // Retrieve all messages from the database
            var messages = _context.ContactMessages
                .Select(m => new MessageVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Subject = m.Subject,
                    Message = m.Message,
                    CreatedAt = m.CreatedAt
                })
                .ToList();

            var model = new MessageIndexVM
            {
                Messages = messages
            };

            return View(model);
        }
        #endregion

        #region Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Find the message by its ID
            var message = _context.ContactMessages.FirstOrDefault(m => m.Id == id);
            if (message == null) return NotFound();

            // Remove the message from the database
            _context.ContactMessages.Remove(message);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            // Find the message by its ID
            var message = _context.ContactMessages
                .Where(m => m.Id == id)
                .Select(m => new MessageVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Subject = m.Subject,
                    Message = m.Message,
                    CreatedAt = m.CreatedAt
                })
                .FirstOrDefault();

            // If the message doesn't exist, return a 404 page
            if (message == null)
            {
                return NotFound();
            }

            // Return the details view with the message
            return View(message);
        }
        #endregion
    }
}
