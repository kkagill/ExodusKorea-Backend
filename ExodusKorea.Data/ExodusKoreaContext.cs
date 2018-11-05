using ExodusKorea.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExodusKorea.Data
{
    public class ExodusKoreaContext : IdentityDbContext<ApplicationUser>
    {
        public ExodusKoreaContext(DbContextOptions options) : base(options) { }

        public DbSet<NewVideo> NewVideos { get; set; }
        public DbSet<CountryInfo> CountryInfo { get; set; }
        public DbSet<PriceInfo> PriceInfo { get; set; }
        public DbSet<VideoComment> VideoComments { get; set; }
        public DbSet<VideoCommentReply> VideoCommentReplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NewVideo>().ToTable("NewVideo");
            modelBuilder.Entity<CountryInfo>().ToTable("CountryInfo");
            modelBuilder.Entity<PriceInfo>().ToTable("PriceInfo");
            modelBuilder.Entity<VideoComment>().ToTable("VideoComment");
            modelBuilder.Entity<VideoCommentReply>().ToTable("VideoCommentReply");
        }
    }
}