using iASignalApi.Models;
using iASignalApi.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace iASignalApi.Repository;

public class StockDbContext(DbContextOptions<StockDbContext> options): DbContext(options)
{
    public DbSet<StockBasic> StockBasics { get; set; }
    public DbSet<StockMarket> StockMarkets { get; set; }
    public DbSet<StockPortfolio> StockPortfolios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<StockMarket>()
            .HasMany(e => e.StockBasics)
            .WithOne(e => e.StockMarket)
            .HasForeignKey(e => e.StockMarketId)
            .IsRequired();

        modelBuilder.Entity<StockPortfolio>()
            .HasMany(e => e.Stocks)
            .WithMany(e => e.StockPortfolios);

        modelBuilder.Entity<StockPortfolio>()
            .HasMany(e => e.Receivers)
            .WithMany(e => e.StockPortfolios);
    }
}