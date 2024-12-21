using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Technify.Entities;
using Technify.Models;
using Technify.Data;
using Technify.Models.Work;
using Microsoft.EntityFrameworkCore;

namespace Technify.Controllers
{
	//[Authorize]
	public class WorkController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _context;

		public WorkController(UserManager<User> userManager, AppDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		#region Index
		
		public IActionResult Index()
		{
            var model = new WorkVM
            {
                Works = _context.Works.Include(w => w.WorkType).ToList()
            };
            return View(model);
        }
		#endregion

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
            var model = new WorkCreateVM
            {
                WorkTypes = _context.WorkTypes
           .Select(wt => new SelectListItem
           {
               Text = wt.Name,
               Value = wt.Id.ToString()
           })
           .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(WorkCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);
           

                model.WorkTypes = _context.WorkTypes
                  .Select(wt => new SelectListItem
                  {
                      Text = wt.Name,
                      Value = wt.Id.ToString()
                  })
                  .ToList();
            

            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User);
            var work = new Work
            {
                Title = model.Title,
                WorkTypeId = model.WorkTypeId,
                Name = model.Name,
                Surname = model.Surname,
                PhoneNumber = model.PhoneNumber,
                GitHubLink = model.GitHubLink,
                CvLink = model.CvLink,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Works.Add(work);
            _context.SaveChanges();

            return RedirectToAction("Index", "Services");
        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            var work = _context.Works
                .Include(w => w.WorkType)
                .FirstOrDefault(w => w.Id == id);

            if (work == null) return NotFound();

            var user = await _userManager.FindByIdAsync(work.UserId);
            var userName = user != null ? user.UserName : "Unknown User";

            var model = new WorkDetailsVM
            {
                Id = work.Id,
                Title = work.Title,
                WorkTypeName = work.WorkType.Name,
                Name = work.Name,
                Surname = work.Surname,
                PhoneNumber = work.PhoneNumber,
                GitHubLink = work.GitHubLink,
                CvLink = work.CvLink,
                CreatedAt = work.CreatedAt,
                UpdatedAt = work.UpdatedAt,
                UserId = work.UserId,
                UserName = userName 
            };

            return View(model);
        }


        #endregion


        #region Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            var work = _context.Works.Find(id);
            if (work == null) return NotFound();
         

            var model = new WorkUpdateVM
            {
                Id = work.Id,                
                Title = work.Title,
                Name = work.Name,
                Surname = work.Surname,
                PhoneNumber = work.PhoneNumber,
                GitHubLink = work.GitHubLink,
                CvLink = work.CvLink,
                CreatedAt = work.CreatedAt,
                WorkTypeId = work.WorkTypeId,
                WorkTypes = _context.WorkTypes.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Update(int id, WorkUpdateVM model)
        {
            model.WorkTypes = _context.WorkTypes.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            if (!ModelState.IsValid)  return View(model);
            

            var work = _context.Works.FirstOrDefault(w => w.Id == id);
            if (work == null)
            {
                return NotFound();
            }
            var workType = _context.WorkTypes.Find(model.WorkTypeId);
            if (workType is null) return NotFound();

            work.Id = id;
            work.Title = model.Title;
            work.Name = model.Name;
            work.Surname = model.Surname;
            work.PhoneNumber = model.PhoneNumber;
            work.GitHubLink = model.GitHubLink;
            work.CvLink = model.CvLink;
            work.WorkTypeId = workType.Id;
            work.UpdatedAt = DateTime.Now;

            _context.Works.Update(work);
            _context.SaveChanges();

            return RedirectToAction("Index","Services");
        }

        #endregion

        #region Delete
        [HttpPost]
		public IActionResult Delete(int id)
		{
			var userId = _userManager.GetUserId(User);
			var work = _context.Works
				.FirstOrDefault(w => w.Id == id && w.UserId == userId);
			if (work == null) return NotFound();

			_context.Works.Remove(work);
			_context.SaveChanges();

			return RedirectToAction("Index","Profile");
		}
		#endregion
	}
}
