using Microsoft.EntityFrameworkCore;
using WebLibrary.Models;

namespace WebLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){            
        }

        public DbSet<Book> books {  get; set; }    
    }
}
