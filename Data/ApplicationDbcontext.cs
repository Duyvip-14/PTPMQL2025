using Microsoft.EntityFrameworkCore;
using DemoMvc551.Models; 
namespace DemoMvc551.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
               public DbSet<Person> Persons { get; set; }
     
}
}