using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class WorkUpdateVM
{
    public int Id { get; set; } 
    
    public string Title { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string GitHubLink { get; set; }
    public string CvLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SelectListItem>? WorkTypes { get; set; }
    [Display(Name = "Work Type ")]
    public int WorkTypeId { get; set; }
}
