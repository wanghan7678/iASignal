using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iASignalApi.Configurations;
using iASignalApi.Constants;
using Microsoft.IdentityModel.Tokens;

namespace iASignalApi.Services;

public interface ITokenHandler
{
    string GetToken(string userId, string username, string[] roles);
    string GetToken(ClaimsIdentity identity);
}

public class JwtHandler : ITokenHandler
{
    private JwtTokenOption _jwtTokenOption;
    private IConfiguration _configuration;

    public JwtHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtTokenOption = new JwtTokenOption();
        _configuration.Bind("JwtBearOptions", _jwtTokenOption);
    }
    
    public string GetToken(string userId, string username, string[] roles)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOption.SecurityKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, userId),
            new(UserConstants.ClaimTypesUserName, username)
        ];
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        JwtSecurityToken token = new(_jwtTokenOption.Issuer, _jwtTokenOption.Audience,
            claims, expires: DateTime.Now.AddHours(10), signingCredentials: signingCredentials);
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return encodedToken;
        
    }

    public string GetToken(ClaimsIdentity identity)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOption.SecurityKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new(_jwtTokenOption.Issuer, _jwtTokenOption.Audience,
            identity.Claims, expires: DateTime.Now.AddHours(10), signingCredentials: signingCredentials);
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return encodedToken;
    }
}