namespace iASignalApi.Models.Requests;

public class UserRegisterRequest
{
    public string Username { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
}