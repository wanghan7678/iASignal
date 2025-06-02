namespace iASignalApi.Models.Dtos;

public class UserResponse
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = [];
}