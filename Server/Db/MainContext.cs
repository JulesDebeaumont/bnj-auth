namespace Server.Db;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

public sealed class MainContext : IdentityDbContext<User>
{
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>(entity =>
            {
                entity.HasMany(p => p.ProjectUsers)
                .WithOne(pu => pu.Project)
                .HasForeignKey(pu => pu.ProjectId)
                .HasPrincipalKey(p => p.Id);

                entity.HasMany(p => p.ProjectFiles)
                .WithOne(pf => pf.Project)
                .HasForeignKey(pf => pf.ProjectId)
                .HasPrincipalKey(p => p.Id);

                entity.HasMany(p => p.Sketches)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .HasPrincipalKey(p => p.Id);
            });

        builder.Entity<ProjectFile>(entity =>
            {
                entity.HasOne(pf => pf.Project)
                .WithMany(p => p.ProjectFiles)
                .HasForeignKey(pj => pj.ProjectId);

                entity.HasOne(pf => pf.User)
                .WithMany(u => u.ProjectFiles)
                .HasForeignKey(pj => pj.UserId);
            });


    }

    public DbSet<Application> Applications { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<QuotationFile> QuotationFiles { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<User> Users { get; set; }

}
