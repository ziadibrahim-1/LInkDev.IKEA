using LinkDev.IKEA.BLL;
using LinkDev.IKEA.DAL;
using LinkDev.IKEA.PL.Extentions;
using LinkDev.IKEA.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace LinkDev.IKEA.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.
            builder.Services.AddWebServvices();

            //builder.Services.AddPersistenceServices(builder.Configuration);
            //builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddMessageing(builder.Configuration);

            builder.Services.AddAuthentication(
            ).AddGoogle(o =>
            {
                IConfiguration GoogleAuth = builder.Configuration.GetSection("Authentication:Google");
                o.ClientId = GoogleAuth["ClientId"]!;
                o.ClientSecret = GoogleAuth["ClientSecret"]!;
            });

            #endregion

            var app = builder.Build();


            #region Database Initializer
            await app.InitializeDatabase();
            #endregion

            #region Http Requsts Pipelines
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            #endregion

            app.Run();
        }
    }
}
