using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technify.Data;
using System.Linq;
using Technify.Models.WorkTypes;

namespace Technify.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var works = _context.Works.ToList();
            var workTypes = _context.WorkTypes.ToList();

            var model = new WorkTypesIndexVM
            {
                WorkTypes = workTypes,
                  Works = works
            };

            return View(model);
        }

        public IActionResult FilterByWorkType(string type)
        {
            
            var works = _context.Works
                                .Where(w => w.WorkType.Name == type)
                                .ToList();
            var model = new WorkTypesIndexVM
            {
                WorkTypes = _context.WorkTypes.ToList(),
                Works = works
            };


            return PartialView("_WorksPartial", model); 
        }

    }
}
