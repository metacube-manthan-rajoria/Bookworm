using Bookworm.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }

    public DbSet<Category> Categories {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category{Id=1, Name="Manthan", DisplayOrder=1},
            new Category{Id=2, Name="Doom", DisplayOrder=2},
            new Category{Id=3, Name="Nyx", DisplayOrder=3}
        );
    }
}
