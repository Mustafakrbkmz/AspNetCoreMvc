using AspNetCoreMvc2.Introduction.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMvc2.Introduction.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) //Dependency Injection
            : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}
