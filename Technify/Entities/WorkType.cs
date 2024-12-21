namespace Technify.Entities
{
	public class WorkType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Work> Works { get; set; } 
	}
}