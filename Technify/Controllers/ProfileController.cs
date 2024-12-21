using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Technify.Data;
using Technify.Entities;

[Authorize]
public class ProfileController : Controller
{
	private readonly AppDbContext _context;

	public ProfileController(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		// Get the logged-in user's ID
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		// Fetch works belonging to the current user
		var userWorks = await _context.Works
			.Include(w => w.WorkType) // Include related WorkType for display
			.Where(w => w.UserId == userId)
			.OrderByDescending(w => w.CreatedAt) // Sort by newest works
			.ToListAsync();

		return View(userWorks);
	}
}
