using DealershipManagement.Common;
using DealershipManagement.Common.Helpers;
using DealershipManagement.DataTransferObjects.Notification;
using DealershipManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;

        public NotificationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> AddNotificationAsync(NotificationRequestDto notificationDto)
        {
            var emailNotificationRequest = new EmailNotificationRequestDto
            {
                ToAddress = notificationDto.ToAddress,
                FromAddress = notificationDto.FromAddress,
                Body = notificationDto.Body,
                IsBodyHtml = notificationDto.IsBodyHtml,
                Subject = notificationDto.Subject,
                IgnoreTemplate = notificationDto.IgnoreTemplate,
                Data = notificationDto.Data,
                Code = notificationDto.Code
            };

            return await this.SendEmailNotificationAsync(emailNotificationRequest);
        }

        public async Task<string> SendEmailNotificationAsync(EmailNotificationRequestDto notification)
        {
            var client = new SendGridClient(configuration.GetValue<string>("SendGridKey"));
            var from = new EmailAddress(notification.FromAddress, "FIA Dealerships");
            var to = new EmailAddress(notification.ToAddress, notification.ToAddress);
            var subject = notification.Subject;
            var htmlContent = notification.IsBodyHtml ? notification.Body : string.Empty;
            var plaintextContent = notification.IsBodyHtml ? string.Empty : notification.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plaintextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                CustomErrors.EmailNotSent.ThrowCustomErrorException(HttpStatusCode.BadRequest);
            else
                return response.Headers.TryGetValues("x-message-id", out var values) ? values.FirstOrDefault() : string.Empty;

            return string.Empty;
        }
    }
}
