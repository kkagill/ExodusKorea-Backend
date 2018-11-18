using ExodusKorea.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExodusKorea.Data
{
    public class ExodusKoreaContext : IdentityDbContext<ApplicationUser>
    {
        public ExodusKoreaContext(DbContextOptions options) : base(options) { }

        public DbSet<VideoPost> VideoPosts { get; set; }
        public DbSet<CountryInfo> CountryInfo { get; set; }
        public DbSet<PriceInfo> PriceInfo { get; set; }
        public DbSet<VideoComment> VideoComments { get; set; }
        public DbSet<VideoCommentReply> VideoCommentReplies { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentReplyLike> CommentReplyLikes { get; set; }
        public DbSet<VideoPostLike> VideoPostLikes { get; set; }
        public DbSet<PI_Rent> PI_Rent { get; set; }
        public DbSet<PI_Groceries> PI_Groceries { get; set; }
        public DbSet<PI_Restaurant> PI_Restaurant { get; set; }
        public DbSet<PI_Etc> PI_Etc { get; set; }
        public DbSet<SalaryInfo> SalaryInfo { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<MinimumCostOfLiving> MinimumCostOfLiving { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CommentLike>()
            .HasKey(c => new { c.VideoCommentId, c.UserId });
            modelBuilder.Entity<CommentReplyLike>()
           .HasKey(c => new { c.VideoCommentReplyId, c.UserId });
            modelBuilder.Entity<VideoPostLike>()
           .HasKey(c => new { c.VideoPostId, c.UserId });

            modelBuilder.Entity<VideoPost>().ToTable("VideoPost");
            modelBuilder.Entity<CountryInfo>().ToTable("CountryInfo");
            modelBuilder.Entity<PriceInfo>().ToTable("PriceInfo");
            modelBuilder.Entity<VideoComment>().ToTable("VideoComment");
            modelBuilder.Entity<VideoCommentReply>().ToTable("VideoCommentReply");
            modelBuilder.Entity<PI_Rent>().ToTable("PI_Rent");
            modelBuilder.Entity<PI_Groceries>().ToTable("PI_Groceries");
            modelBuilder.Entity<PI_Restaurant>().ToTable("PI_Restaurant");
            modelBuilder.Entity<PI_Etc>().ToTable("PI_Etc");
            modelBuilder.Entity<SalaryInfo>().ToTable("SalaryInfo");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<MinimumCostOfLiving>().ToTable("MinimumCostOfLiving");
        }
    }
}