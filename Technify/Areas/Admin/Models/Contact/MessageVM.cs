using System.ComponentModel.DataAnnotations;

public class MessageVM
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Subject is required")]
    [StringLength(10, ErrorMessage = "Subject cannot exceed 10 characters.")]

    public string Subject { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
