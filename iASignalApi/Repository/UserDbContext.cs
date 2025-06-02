using iASignalApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iASignalApi.Repository;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<User, IdentityRole, string>(options)
{
    private DbSet<User> Users { get; set; }
}