using iASignalApi.Constants;
using iASignalApi.Models;
using iASignalApi.Models.Requests;
using iASignalApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iASignalApi.Controllers;

[ApiController]
[Route("api/stock")]
public class StockBasicController(IStockRepository stockRepository) : ControllerBase
{
    private readonly IStockRepository _stockRepository = stockRepository;

    [HttpPost]
    [Route("market/add")]
    [Authorize(UserConstants.RoleAdmin)]
    public async Task<IActionResult> AddNewMarket([FromBody] StockMarketRequest stockMarketRequest)
    {
        var idString = Guid.NewGuid().ToString();
        var market = new StockMarket()
        {
            Name = stockMarketRequest.Name,
            Exchange = stockMarketRequest.Exchange,
            Id = idString,
            Location = stockMarketRequest.Location,
        };
        await _stockRepository.StockDbContext.AddAsync(market);
        await _stockRepository.StockDbContext.SaveChangesAsync();
        market = await _stockRepository.StockDbContext.StockMarkets.FindAsync(idString);
        return Ok(market);
    }

    [HttpPost]
    [Route("basic/add")]
    [Authorize(UserConstants.RoleAdmin)]
    public async Task<IActionResult> AddNewStockBasic([FromBody] StockBasicRequest stockBasicRequest)
    {
        var id = Guid.NewGuid().ToString();
        var basic = new StockBasic()
        {
            Name = stockBasicRequest.Name,
            Id = id,
            IsHs = stockBasicRequest.IsHs,
            Status = StockConstants.StockStatusList,
            StockMarketId = stockBasicRequest.MarketId,
            TsCode = stockBasicRequest.TsCode
        };
        await _stockRepository.StockDbContext.StockBasics.AddAsync(basic);
        await _stockRepository.StockDbContext.SaveChangesAsync();
        basic = await _stockRepository.StockDbContext.StockBasics.AsNoTracking()
            .Include(e => e.StockMarket)
            .FirstOrDefaultAsync(e => e.Id == id);
        return Ok(basic);
    }
    
    
}