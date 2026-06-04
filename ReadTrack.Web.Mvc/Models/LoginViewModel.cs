using System.ComponentModel.DataAnnotations;

namespace ReadTrack.Web.Mvc.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? Password { get; set; }
}