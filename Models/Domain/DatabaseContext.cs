using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
                : base(options)
        {

        }
        public DbSet<AngularProduct> angularProductsTbl { get; set; }
    }
}
