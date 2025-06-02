namespace iASignalApi.Models;

public class StockMarket
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Exchange { get; set; }
    public string Location { get; set; }
    
    public ICollection<StockBasic> StockBasics { get; set; }
}