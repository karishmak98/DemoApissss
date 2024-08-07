using Microsoft.EntityFrameworkCore;
using WebApiPractice.Models;

namespace WebApiPractice.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Product> Products { get; set; }


    }
}
