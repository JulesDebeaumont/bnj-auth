namespace Server.DAL.Db;

using Microsoft.EntityFrameworkCore;
using Server.Models;

public sealed class MainContext : DbContext
{
  public MainContext(DbContextOptions<MainContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }

  public DbSet<Application> Applications { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<Organization> Organizations { get; set; }
  public DbSet<Quotation> Quotations { get; set; }
  public DbSet<QuotationFile> QuotationFile { get; set; }
  public DbSet<Subscription> Subscriptions { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<UserAvatar> UserAvatar { get; set; }

}