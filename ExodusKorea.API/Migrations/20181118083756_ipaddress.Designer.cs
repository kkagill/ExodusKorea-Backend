﻿// <auto-generated />
using ExodusKorea.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ExodusKorea.API.Migrations
{
    [DbContext(typeof(ExodusKoreaContext))]
    [Migration("20181118083756_ipaddress")]
    partial class ipaddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExodusKorea.Model.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.HasKey("CityId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.CommentLike", b =>
                {
                    b.Property<long>("VideoCommentId");

                    b.Property<string>("UserId");

                    b.HasKey("VideoCommentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentLikes");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.CommentReplyLike", b =>
                {
                    b.Property<long>("VideoCommentReplyId");

                    b.Property<string>("UserId");

                    b.HasKey("VideoCommentReplyId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentReplyLikes");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.CountryInfo", b =>
                {
                    b.Property<int>("CountryInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CapitalCity");

                    b.Property<string>("Country");

                    b.Property<string>("CountryInEng");

                    b.Property<string>("CountryLink");

                    b.Property<string>("Currency");

                    b.Property<string>("Languages");

                    b.Property<string>("MajorCities");

                    b.Property<string>("PerCapitaGDP");

                    b.Property<string>("Population");

                    b.HasKey("CountryInfoId");

                    b.ToTable("CountryInfo");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.MinimumCostOfLiving", b =>
                {
                    b.Property<int>("MinimumCostOfLivingId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cell");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<int>("CountryInfoId");

                    b.Property<string>("Etc");

                    b.Property<decimal>("Food");

                    b.Property<decimal>("Internet");

                    b.Property<string>("IpAddress");

                    b.Property<string>("NickName");

                    b.Property<decimal>("Rent");

                    b.Property<decimal>("Transportation");

                    b.HasKey("MinimumCostOfLivingId");

                    b.ToTable("MinimumCostOfLiving");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.Notification", b =>
                {
                    b.Property<long>("NotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateCreated");

                    b.Property<bool>("HasRead");

                    b.Property<string>("NickName");

                    b.Property<string>("UserId");

                    b.Property<long>("VideoCommentId");

                    b.Property<long>("VideoCommentReplyId");

                    b.Property<int>("VideoPostId");

                    b.Property<string>("YouTubeVideoId");

                    b.HasKey("NotificationId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Etc", b =>
                {
                    b.Property<int>("PI_EtcId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Bus");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<decimal>("Gas");

                    b.Property<decimal>("Internet");

                    b.Property<int>("PriceInfoId");

                    b.Property<decimal>("Subway");

                    b.HasKey("PI_EtcId");

                    b.HasIndex("PriceInfoId");

                    b.ToTable("PI_Etc");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Groceries", b =>
                {
                    b.Property<int>("PI_GroceriesId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Apple");

                    b.Property<decimal>("ChickenBreasts");

                    b.Property<decimal>("Cigarettes");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<decimal>("Eggs");

                    b.Property<decimal>("Milk");

                    b.Property<decimal>("Potatoes");

                    b.Property<int>("PriceInfoId");

                    b.Property<decimal>("Water");

                    b.HasKey("PI_GroceriesId");

                    b.HasIndex("PriceInfoId");

                    b.ToTable("PI_Groceries");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Rent", b =>
                {
                    b.Property<int>("PI_RentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<decimal>("OneBedRoomCenter");

                    b.Property<decimal>("OneBedRoomOutside");

                    b.Property<int>("PriceInfoId");

                    b.Property<decimal>("TwoBedRoomCenter");

                    b.Property<decimal>("TwoBedRoomOutside");

                    b.HasKey("PI_RentId");

                    b.HasIndex("PriceInfoId");

                    b.ToTable("PI_Rent");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Restaurant", b =>
                {
                    b.Property<int>("PI_RestaurantId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("BigMacMeal");

                    b.Property<decimal>("Cappuccino");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<decimal>("MealPerOne");

                    b.Property<int>("PriceInfoId");

                    b.HasKey("PI_RestaurantId");

                    b.HasIndex("PriceInfoId");

                    b.ToTable("PI_Restaurant");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PriceInfo", b =>
                {
                    b.Property<int>("PriceInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("CostOfLivingIndex");

                    b.Property<string>("Country");

                    b.Property<decimal>("GroceriesIndex");

                    b.Property<decimal>("RentIndex");

                    b.Property<decimal>("RestaurantPriceIndex");

                    b.HasKey("PriceInfoId");

                    b.ToTable("PriceInfo");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.SalaryInfo", b =>
                {
                    b.Property<int>("SalaryInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("Currency");

                    b.Property<decimal>("High");

                    b.Property<bool>("IsDisplayable");

                    b.Property<decimal>("Low");

                    b.Property<decimal>("Median");

                    b.Property<string>("Occupation");

                    b.Property<int>("VideoPostId");

                    b.HasKey("SalaryInfoId");

                    b.ToTable("SalaryInfo");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoComment", b =>
                {
                    b.Property<long>("VideoCommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorDisplayName");

                    b.Property<string>("Comment");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<int>("Likes");

                    b.Property<string>("UserId");

                    b.Property<int>("VideoPostId");

                    b.HasKey("VideoCommentId");

                    b.HasIndex("VideoPostId");

                    b.ToTable("VideoComment");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoCommentReply", b =>
                {
                    b.Property<long>("VideoCommentReplyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorDisplayName");

                    b.Property<string>("Comment");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<int>("Likes");

                    b.Property<string>("RepliedTo");

                    b.Property<string>("UserId");

                    b.Property<long>("VideoCommentId");

                    b.HasKey("VideoCommentReplyId");

                    b.HasIndex("VideoCommentId");

                    b.ToTable("VideoCommentReply");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoPost", b =>
                {
                    b.Property<int>("VideoPostId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("CountryInEng");

                    b.Property<int>("Likes");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UploadedDate");

                    b.Property<string>("Uploader");

                    b.Property<string>("YouTubeVideoId");

                    b.HasKey("VideoPostId");

                    b.ToTable("VideoPost");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoPostLike", b =>
                {
                    b.Property<int>("VideoPostId");

                    b.Property<string>("UserId");

                    b.HasKey("VideoPostId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("VideoPostLikes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictApplication", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId")
                        .IsRequired();

                    b.Property<string>("ClientSecret");

                    b.Property<string>("DisplayName");

                    b.Property<string>("PostLogoutRedirectUris");

                    b.Property<string>("RedirectUris");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationId");

                    b.Property<string>("Scopes");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("OpenIddictAuthorizations");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictScope", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("OpenIddictScopes");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationId");

                    b.Property<string>("AuthorizationId");

                    b.Property<string>("Ciphertext");

                    b.Property<DateTimeOffset?>("CreationDate");

                    b.Property<DateTimeOffset?>("ExpirationDate");

                    b.Property<string>("Hash");

                    b.Property<string>("Status");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("AuthorizationId");

                    b.HasIndex("Hash")
                        .IsUnique()
                        .HasFilter("[Hash] IS NOT NULL");

                    b.ToTable("OpenIddictTokens");
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.CommentLike", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser", "User")
                        .WithMany("CommentLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExodusKorea.Model.Entities.VideoComment", "VideoComment")
                        .WithMany("CommentLikes")
                        .HasForeignKey("VideoCommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.CommentReplyLike", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser", "User")
                        .WithMany("CommentReplyLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExodusKorea.Model.Entities.VideoCommentReply", "VideoCommentReply")
                        .WithMany("CommentReplyLikes")
                        .HasForeignKey("VideoCommentReplyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Etc", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.PriceInfo", "PriceInfo")
                        .WithMany()
                        .HasForeignKey("PriceInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Groceries", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.PriceInfo", "PriceInfo")
                        .WithMany()
                        .HasForeignKey("PriceInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Rent", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.PriceInfo", "PriceInfo")
                        .WithMany()
                        .HasForeignKey("PriceInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.PI_Restaurant", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.PriceInfo", "PriceInfo")
                        .WithMany()
                        .HasForeignKey("PriceInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoComment", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.VideoPost", "VideoPost")
                        .WithMany()
                        .HasForeignKey("VideoPostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoCommentReply", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.VideoComment", "VideoComment")
                        .WithMany("VideoCommentReplies")
                        .HasForeignKey("VideoCommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExodusKorea.Model.Entities.VideoPostLike", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser", "User")
                        .WithMany("VideoPostLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExodusKorea.Model.Entities.VideoPost", "VideoPost")
                        .WithMany("VideoPostLikes")
                        .HasForeignKey("VideoPostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ExodusKorea.Model.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenIddict.Models.OpenIddictAuthorization", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
