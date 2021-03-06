﻿using ExodusKorea.API.Exceptions;
using ExodusKorea.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using ExodusKorea.API.Services.Interfaces;
using ExodusKorea.Model;

namespace ExodusKorea.API.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly ExodusKoreaContext _context;

        public MessageService(IHttpContextAccessor httpContextAccessor,
                              IConfiguration config,
                              IOptions<AppSettings> appSettings,
                              ExodusKoreaContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _appSettings = appSettings.Value;
            _context = context;
        }

        public Task SendEmailAsync(string from, string to, string subject, string body, string htmlBody)
        {
            if (_appSettings.Environment.Equals("Development"))
                to = "admin@talchoseon.com";

            var message = new MimeMessage();

            if (from != null && from.Length > 0)
                message.From.Add(new MailboxAddress(from));
            if (to != null && to.Length > 0)
                message.To.Add(new MailboxAddress(to));                  

            message.Subject = subject;
            var builder = new BodyBuilder() { TextBody = body };

            if (!string.IsNullOrEmpty(htmlBody))
                builder.HtmlBody = htmlBody;

            message.Body = builder.ToMessageBody();

            // Send email to the user
            using (var client = new SmtpClient())
            {
                client.Connect("in-v3.mailjet.com", 587);
                client.Authenticate("fdbc73a4f6e45650df958986c1f86ab8", "747e139b010e9f4316f53b674be8f334");
                client.Send(message);
                client.Disconnect(true);
            }

            return Task.FromResult(1);
        }
    }
}
