namespace iASignalApi.Models.Requests;

public class StockBasicRequest
{
    public string Name { get; set; }
    public string MarketId { get; set; }
    public string TsCode { get; set; }
    public bool IsHs { get; set; }
}