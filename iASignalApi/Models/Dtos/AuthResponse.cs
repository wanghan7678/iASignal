namespace iASignalApi.Models.Dtos;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string Id { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; }
}