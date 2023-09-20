using Microsoft.EntityFrameworkCore;
using SipayTestCase.Domain.Entities;

namespace SipayTestCase.Persistence.Context;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var cnnString =
                "User ID=postgres;Password=2212;Host=localhost;Port=5432;Database=SipayTestCase;Pooling=true;";
            optionsBuilder.UseLazyLoadingProxies()
                .UseNpgsql(cnnString, o => o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<User> Users { get; set; }
}