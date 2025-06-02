using iASignalApi.Models.Requests;
using Microsoft.AspNetCore.Identity;

namespace iASignalApi.Models;

public class User: IdentityUser
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    
    public ICollection<StockPortfolio> StockPortfolios { get; set; }
}