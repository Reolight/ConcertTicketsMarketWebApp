using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using ConcertTicketsMarketWebApp.Data;
using ConcertTicketsMarketWebApp.Areas.Identity.Data;
using ConcertTicketsMarketWebApp.Services.EmailService;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;

namespace ConcertTicketsMarketWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuring two databases in ext. method.
            builder.Services.AddDbContextForUsersAndData<IdentityContext, AppDbContext, AppUser>(builder.Configuration);

            // Configuring Identity and roles
            builder.Services.AddIdentityWithJwtAndRoles<AppUser, IdentityContext>();

            // Configuring email service
            builder.Services.AddEmailService().WithOptions(builder.Configuration);

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Here are roles added via extension method
            app.UseDefinedRoles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}