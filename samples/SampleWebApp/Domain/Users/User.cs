using System.ComponentModel.DataAnnotations;

namespace SampleWebApp.Domain.Users;

public class User
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    
    public bool IsActive { get; set; }
}