using System.Reflection;
using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Data.Identity;
using ConcertTicketsMarketWebApp;
using EmailService;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();

builder.Services.AddDbContextForUsersAndData(builder.Configuration);
builder.Services.AddIdentityWithJwtAndRoles();

builder.Services.Configure<EmailServiceConfigurations>(
    builder.Configuration.GetSection(nameof(EmailServiceConfigurations)));
builder.Services.AddTransient<IEmailService, EmailService.EmailService>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(MapsterConfigurationExtenstion)) 
                                        ?? throw new NullReferenceException("No MediatR services found")));
builder.Services.AddMapsterConfiguration();
builder.Services.AddMapster();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDefinedRoles();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();
