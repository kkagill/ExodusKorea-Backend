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
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<MinimumCostOfLiving> MinimumCostOfLiving { get; set; }
        public DbSet<NewsDetail> NewsDetail { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Career> Career { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CountryInfoKOTRA> CountryInfoKOTRA { get; set; }
        public DbSet<MyVideos> MyVideos { get; set; }
        public DbSet<SiteException> SiteException { get; set; }
        public DbSet<HttpResponseException> HttpResponseException { get; set; }
        public DbSet<LoginSession> LoginSession { get; set; }
        public DbSet<WithdrawUser> WithdrawUser { get; set; }
        public DbSet<JobSite> JobSite { get; set; }
        public DbSet<UploadVideo> UploadVideo { get; set; }

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
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<MinimumCostOfLiving>().ToTable("MinimumCostOfLiving");
            modelBuilder.Entity<NewsDetail>().ToTable("NewsDetail");
            modelBuilder.Entity<News>().ToTable("News");   
            modelBuilder.Entity<Career>().ToTable("Career");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<CountryInfoKOTRA>().ToTable("CountryInfoKOTRA");
            modelBuilder.Entity<MyVideos>().ToTable("MyVideo");
            modelBuilder.Entity<SiteException>().ToTable("Log_SiteException");
            modelBuilder.Entity<HttpResponseException>().ToTable("Log_HttpResponseException");
            modelBuilder.Entity<LoginSession>().ToTable("Log_LoginSession");
            modelBuilder.Entity<WithdrawUser>().ToTable("Log_WithdrawUser");
            modelBuilder.Entity<JobSite>().ToTable("JobSite");
            modelBuilder.Entity<UploadVideo>().ToTable("UploadVideo");
        }
    }
}