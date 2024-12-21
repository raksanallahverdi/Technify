namespace Technify.Entities
{
	public class Work
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Name { get; set; } 
		public string Surname { get; set; } 
		public string PhoneNumber { get; set; } 
		public string GitHubLink { get; set; }
		public string UserId { get; set; }
		public int WorkTypeId { get; set; } 
		public WorkType WorkType { get; set; } 
		public string CvLink { get; set; } 
		public DateTime CreatedAt { get; set; } 
		public DateTime? UpdatedAt { get; set; } 
	}
}
