using Bookworm.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }

    //public DbSet<Category> Category {get; set;}
}
