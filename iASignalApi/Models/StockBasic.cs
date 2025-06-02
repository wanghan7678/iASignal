using iASignalApi.Models.Requests;

namespace iASignalApi.Models;

public class StockBasic
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TsCode { get; set; }
    public string Status { get; set; }
    public bool IsHs { get; set; }
    public string StockMarketId { get; set; }
    public StockMarket StockMarket { get; set; }
    
    public ICollection<StockPortfolio> StockPortfolios { get; set; }
}