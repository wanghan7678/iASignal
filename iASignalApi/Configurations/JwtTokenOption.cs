namespace iASignalApi.Configurations;

public class JwtTokenOption
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
}