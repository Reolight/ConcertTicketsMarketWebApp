using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ConcertTicketsMarketWebApp.Data;
using ConcertTicketsMarketWebApp.Areas.Identity.Data;
using ConcertTicketsMarketWebApp.Services.EmailService;

namespace ConcertTicketsMarketWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            (var identityConnectionString, var dataContext) = (
                    builder.Configuration.GetConnectionString("IdentityContextConnection"),
                    builder.Configuration.GetConnectionString("DataContextConnection")
            );

            var EmailServiceConfigs = builder.Configuration
                .GetSection("EmailServiceConfigurations")
                .Get<EmailServiceConfigurations>() ?? throw new InvalidOperationException("Email configs are can not be read");

            if (string.IsNullOrEmpty(identityConnectionString) || string.IsNullOrEmpty(dataContext))
                throw new InvalidOperationException("Detected empty connection string...");

            builder.Services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(identityConnectionString));

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddSingleton(new EmailService(EmailServiceConfigs));
            // Add services to the container.

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}