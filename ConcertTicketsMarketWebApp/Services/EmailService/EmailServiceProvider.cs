namespace ConcertTicketsMarketWebApp.Services.EmailService
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection WithOptions(this IServiceCollection services, IConfiguration config)
            => services.Configure<EmailServiceConfigurations>(config.GetSection("EmailServiceConfigurations"));
            
        public static IServiceCollection AddEmailService(this IServiceCollection services)
            => services.AddTransient<IEmailService, EmailService>();
    }
}