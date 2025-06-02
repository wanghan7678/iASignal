namespace iASignalApi.Models.Requests;

public class StockPortfolio
{
    public string Id { get; set; }
    public ICollection<User> Receivers { get; set; }
    
    public DateTime? StartDate { get; set; } 
    public DateTime? EndDate { get; set; }
    
    public DateTime? ReportDate { get; set; }
    
    public string RecommendType { get; set; }
    
    public string Recommend { get; set; }
    
    
    public ICollection<StockBasic> Stocks { get; set; }
}