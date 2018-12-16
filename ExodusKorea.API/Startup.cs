using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using ExodusKorea.API.Exceptions;
using ExodusKorea.API.Provider;
using ExodusKorea.API.Services;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Data;
using ExodusKorea.Data.Interfaces;
using ExodusKorea.Data.Repositories;
using ExodusKorea.Model;
using ExodusKorea.Model.Entities;
using ExodusKorea.Model.ViewModels.Mapping;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace ExodusKorea.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add database configurations  
            services.AddDbContext<ExodusKoreaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ExodusKorea"),
                b => b.MigrationsAssembly("ExodusKorea.API"));
                options.UseOpenIddict();
            });

            // Add membership
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;      
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = null;

                // Confirmation email required for new account
                options.SignIn.RequireConfirmedEmail = true;

                // Lockout settings
                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                //options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ExodusKoreaContext>()
                .AddDefaultTokenProviders();

            // Register the OAuth2 validation handler.
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer(options =>
                   {
                       options.Audience = "resource_server";
                       options.Authority = "http://localhost:18691/";
                       //options.Authority = "https://exoduskoreaapi.azurewebsites.net/";
                       options.RequireHttpsMetadata = false;
                       options.IncludeErrorDetails = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           NameClaimType = OpenIdConnectConstants.Claims.Subject,
                           RoleClaimType = OpenIdConnectConstants.Claims.Role
                       };
                   });

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            }); 

            services.AddOpenIddict()
                // Register the OpenIddict core services.
                .AddCore(options =>
                {
                    // Register the Entity Framework stores and models.
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ExodusKoreaContext>();
                })
                // Register the OpenIddict server handler.
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();
                    // Enable the token endpoint.
                    options.EnableTokenEndpoint("/connect/token");
                    // Enable the password and the refresh token flows.
                    options.AllowPasswordFlow()
                           .AllowRefreshTokenFlow()                           
                           .SetAccessTokenLifetime(TimeSpan.FromSeconds(10)); // default is 5 minutes
                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    options.AcceptAnonymousClients();
                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();
                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    //
                    options.UseJsonWebTokens();
                    options.AddEphemeralSigningKey();
                });

            // For confirming email
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });

            // Automapper   
            Mapper.Initialize(config =>
            {
                config.AddProfile(new MappingProfile());
            });

            services.AddCors();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(config =>
            {
                // Custom Global Exception Handler
                config.Filters.Add(typeof(CustomExceptionFilter));
                // Force https in production mode
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());               
#endif
            })
                .AddJsonOptions(options =>
                {
                    // Force Camel Case to JSON
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.Configure<GoogleReCaptcha>(Configuration.GetSection("GoogleReCaptcha"));
            services.Configure<YoutubeData>(Configuration.GetSection("YoutubeData"));

            // Repositories
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<ICardDetailRepository, CardDetailRepository>();
            services.AddScoped<IVideoPostRepository, VideoPostRepository>();
            services.AddScoped<IVideoCommentRepository, VideoCommentRepository>();
            services.AddScoped<IVideoCommentReplyRepository, VideoCommentReplyRepository>();
            services.AddScoped<IVideoPostLikeRepository, VideoPostLikeRepository>();
            services.AddScoped<IVideoCommentLikeRepository, VideoCommentLikeRepository>();
            services.AddScoped<IVideoCommentReplyLikeRepository, VideoCommentReplyLikeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMinimumCostOfLivingRepository, MinimumCostOfLivingRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsDetailRepository, NewsDetailRepository>();
            services.AddScoped<ICountryInfoRepository, CountryInfoRepository>();
            services.AddScoped<IMyVideosRepository, MyVideosRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            // Services            
            services.AddTransient<DbInitializer>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ICurrencyRatesService, CurrencyRatesService>();
            services.AddTransient<IGoogleRecaptchaService, GoogleRecaptchaService>();
            services.AddTransient<IYouTubeService, YoutubeService>();
            services.AddTransient<IClientIPService, ClientIPService>();
            services.AddTransient<ILogDataService, LogDataService>();
            // Without this controller actions are not forbidden if other roles are trying to access
            services.AddSingleton<IAuthenticationSchemeProvider, CustomAuthenticationSchemeProvider>();
            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInitializer seeder, IAntiforgery antiforgery)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            app.UseStaticFiles();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200");                
                //builder.WithOrigins("https://test2-5d022.firebaseapp.com");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        //context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                    }
                });
            });

            app.Use(next => context =>
            {
                if (context.Request.Path.Value.IndexOf("/", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
                }

                return next(context);
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("DefaultApi", "api/{controller}/{id?}");
            });

            app.UseWelcomePage();

            //seeder.InitializeData().Wait();
        }
    }
}
