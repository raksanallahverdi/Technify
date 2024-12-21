public class WorkDetailsVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string WorkTypeName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string GitHubLink { get; set; }
    public string CvLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
}
