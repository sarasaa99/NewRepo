using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shatably.BLL;
using Shatably.Data.Context;
using Shatably.Data.Models;
using Shatably.Data.Validators;
using Shatably.Service;
using Shatably.Service.Email;
using System.Net;
using System.Text;

namespace Shatably.Web
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
            services.AddDbContext<ShatblyDbContext>(
                 option => option.UseSqlServer(Configuration.GetConnectionString("ShatablyConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ShatblyDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization((options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin", "User", "Company");
                    });
            }));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
                };
            });

            #region EmailConfig

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            services.AddScoped<IEmailSender, EmailSender>();

            #endregion EmailConfig

            services.AddHttpContextAccessor();

            services.AddDataProtection();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc().AddFluentValidation();

            #region Dependency Injection

            #region Repositories

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IGalleryReposatory, GalleryRepository>();
            services.AddScoped<ICompanyReposatory, CompanyRepository>();
            services.AddScoped<IReviewsReposatory, ReviewsRepository>();

            #endregion Repositories

            #region Services

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGalleryService, GalleryService>();
            //services.AddScoped<IReviewsService, ReviewsService>();

            #endregion Services

            #region Validators

            services.AddTransient<IValidator<Company>, CompanyValidator>();

            #endregion Validators

            #endregion Dependency Injection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}