using System.Net;
using System.Security.Claims;
using AutoMapper;
using iASignalApi.Constants;
using iASignalApi.Models;
using iASignalApi.Models.Dtos;
using iASignalApi.Models.Requests;
using iASignalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;

namespace iASignalApi.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController:ControllerBase
{
    private UserManager<User> _userManager;
    private RoleManager<IdentityRole> _roleManager;
    private SignInManager<User> _signInManager;
    private ITokenHandler _tokenHandler;
    private IMapper _mapper;

    public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager,
        ITokenHandler tokenHandler, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest userLoginRequest)
    {
        var user = await _userManager.FindByNameAsync(userLoginRequest.Username);
        if (user == null)
        {
            return BadRequest("User cannot be found");
        }

        var result = await _signInManager.PasswordSignInAsync(user, userLoginRequest.Password,
            userLoginRequest.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return BadRequest("username password is not matched.");
        }
        var identity = new ClaimsIdentity(IdentityConstants.BearerScheme);
        identity.AddClaim(new Claim(UserConstants.ClaimTypesUserId, user.Id));
        identity.AddClaim(new Claim(UserConstants.ClaimTypesUserName, user.UserName));
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(UserConstants.ClaimTypesRole, role));
        }
        //var token = _tokenHandler.GetToken(user.Id, user.UserName, roles.ToArray());

        var token = _tokenHandler.GetToken(identity);
        var authResponse = new AuthResponse()
        {
            AccessToken = token,
            ExpiresAt = DateTime.Now.AddHours(10),
            TokenType = "Bear",
            RefreshToken = Guid.NewGuid().ToString()
        };
        
        return Ok(authResponse);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> UserRegister([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var user = new User()
        {
            UserName = userRegisterRequest.Username,
            PhoneNumber = userRegisterRequest.Phone,
            LastName = userRegisterRequest.LastName,
            FirstName = userRegisterRequest.FirstName,
            Email = userRegisterRequest.Email
        };
        var result = await _userManager.CreateAsync(user, userRegisterRequest.Password);
        if (!result.Succeeded) return BadRequest("User Registration Failed");
        var role = await _roleManager.FindByNameAsync(userRegisterRequest.Role);
        if (role is null)
        {
            return BadRequest("Role cannot be found.");
        }
        await _userManager.AddToRoleAsync(user, userRegisterRequest.Role);
        return Ok("Successful create the user");

    }

    [HttpPost]
    [Authorize(UserConstants.RoleAdmin)]
    [Route("addRole")]
    public async Task<IActionResult> AddRoles([FromBody] UserRoleRequest userRoleRequest)
    {
        var roleName = userRoleRequest.RoleName;
        if (roleName.Equals(UserConstants.RoleGuest, StringComparison.OrdinalIgnoreCase))
        {
           await _roleManager.CreateAsync(new IdentityRole() { Name = UserConstants.RoleGuest });
           return Ok("Successfully add role: " + roleName);
        }

        if (roleName.Equals(UserConstants.RoleUser, StringComparison.OrdinalIgnoreCase))
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = UserConstants.RoleUser });
            return Ok("Successfully add role: " + roleName);
        }

        if (roleName.Equals(UserConstants.RoleAdmin, StringComparison.OrdinalIgnoreCase))
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = UserConstants.RoleAdmin });
            return Ok("Successfully add role: " + roleName);
        }

        return BadRequest("role name: " + roleName + " is illegal.");
    }

    [HttpGet("all")]
    [Authorize(UserConstants.RoleUser)]
    public async Task<IActionResult> GetUsersAll()
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync();
        var results = new List<UserResponse>();
        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserResponse>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDto.Roles.AddRange(roles);
        }
        return Ok(users);
    }
}