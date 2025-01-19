using Microsoft.EntityFrameworkCore;

namespace Bookworm.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    {
        
    }
}
