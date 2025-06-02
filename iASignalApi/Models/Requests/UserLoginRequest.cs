using System.ComponentModel.DataAnnotations;

namespace iASignalApi.Models.Requests;

public class UserLoginRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}