using ExodusKorea.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExodusKorea.Data
{
    public class ExodusKoreaContext : IdentityDbContext<ApplicationUser>
    {
        public ExodusKoreaContext(DbContextOptions options) : base(options) { }

        //public DbSet<Course> Courses { get; set; }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Course>().ToTable("Course");           
        }
    }
}