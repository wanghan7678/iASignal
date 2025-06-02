using System.Text;
using iASignalApi.Configurations;
using iASignalApi.Constants;
using iASignalApi.Models;
using iASignalApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace iASignalApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("AnyPolicy", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });
        });
    }

    public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("Default");
        service.AddDbContext<StockDbContext>(options => options.UseMySql(connString, ServerVersion.AutoDetect(connString)));
        service.AddDbContext<UserDbContext>(options =>
            options.UseMySql(connString, ServerVersion.AutoDetect(connString)));
    }

    public static void ConfigureIdentity(this IServiceCollection service)
    {
        service.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<UserDbContext>();
    }

    public static void ConfigureAuthentication(this IServiceCollection service, IConfiguration configuration)
    {
        var jwtTokenOption = new JwtTokenOption();
        configuration.Bind("JwtBearOptions", jwtTokenOption);
        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtTokenOption.Issuer,
                ValidAudience = jwtTokenOption.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOption.SecurityKey)),
            };

        });
    }

    public static void ConfigurePolicy(this IServiceCollection service)
    {
        service.AddAuthorizationBuilder()
            .AddPolicy(UserConstants.RoleGuest, policy => policy.RequireRole(UserConstants.RoleGuest, UserConstants.RoleUser, UserConstants.RoleAdmin))
            .AddPolicy(UserConstants.RoleUser,
                policy => policy.RequireRole(UserConstants.RoleAdmin, UserConstants.RoleUser))
            .AddPolicy(UserConstants.RoleAdmin,
                policy => policy.RequireRole( UserConstants.RoleAdmin));
    }
    
    public static void ConfigureAuthSwaggerGen(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Please input token, format: Bearer jwtToken",
                Name="Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                       Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, []
                }
            });
        });
    }
}