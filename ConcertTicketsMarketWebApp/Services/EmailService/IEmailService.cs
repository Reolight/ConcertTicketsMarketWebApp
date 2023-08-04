﻿namespace ConcertTicketsMarketWebApp.Services.EmailService
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(EmailBoxInfo receiver, Email email);
    }
}
