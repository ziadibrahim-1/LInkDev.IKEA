using LinkDev.IKEA.BLL.Services.EmailService;
using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.DAL.Persistence.Data;
using LinkDev.IKEA.PL.Helper;
using LinkDev.IKEA.PL.Mapping.Profiles;
using LinkDev.IKEA.PL.Settings;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace LinkDev.IKEA.PL
{
    public static class DepenndancyInjection
    {
        public static IServiceCollection AddWebServvices(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Add AutoMapper
            services.AddAutoMapper(M => { },typeof(EmployeeProfile));

            services.AddScoped<IEmailSender, EmailService>();
            

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });
            return services;
        }
        public static IServiceCollection AddMessageing(this IServiceCollection services , IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<SMSSettings>(configuration.GetSection("Twilio"));
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISMSService, SMSService>();
            return services;
        }
    }
}
