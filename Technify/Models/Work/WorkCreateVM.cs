using Microsoft.AspNetCore.Mvc.Rendering;

namespace Technify.Models.Work
{
	public class WorkCreateVM
	{
		public string Title { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
		public string GitHubLink { get; set; }
		public string CvLink { get; set; }
		public int WorkTypeId { get; set; } 
		public List<SelectListItem>? WorkTypes { get; set; }
	}
}
