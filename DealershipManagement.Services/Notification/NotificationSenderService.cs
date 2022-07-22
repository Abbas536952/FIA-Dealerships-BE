using DealershipManagement.DataAccess.Helpers;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Notification;
using DealershipManagement.DataTransferObjects.Services.Notification;
using DealershipManagement.Entities.Notification;
using DealershipManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Notification
{
    public class NotificationSenderService : INotificationSenderService
    {
        private readonly ITemplateMapper templateMapper;
        private readonly AuditableRepository<NotificationTemplate> notificationTemplateRepository;
        private readonly INotificationService notificationService;
        private readonly string emailSender;

        public NotificationSenderService(
            ITemplateMapper templateMapper,
            AuditableRepository<NotificationTemplate> notificationTemplateRepository,
            IConfiguration configuration,
            INotificationService notificationService)
        {
            this.templateMapper = templateMapper;
            this.emailSender = configuration["EmailSender"];
            this.notificationTemplateRepository = notificationTemplateRepository;
            this.notificationService = notificationService;
        }

        public async Task<string> SendAccountActivationEmailAsync(AccountActivationNotificationRequestDto request)
        {
            var template = await this.notificationTemplateRepository.GetAll()
                .FirstOrDefaultAsync(nt => nt.Name == EmailTemplateNames.AccountActivationTemplate);

            request.Template = template.Template;

            var data = new NotificationRequestDto
            {
                ToAddress = request.EmailAddress,
                FromAddress = this.emailSender,
                IgnoreTemplate = true,
                IsBodyHtml = true,
                Body = this.templateMapper.MapToAccountActivationTemplateData(request),
                Subject = EmailSubjects.AccountActivation
            };

            return await this.notificationService.AddNotificationAsync(data);
        }

        public async Task<string> SendForgotPasswordEmailAsync(ForgotPasswordNotificationRequestDto request)
        {
            var template = await this.notificationTemplateRepository.GetAll()
                .FirstOrDefaultAsync(nt => nt.Name == EmailTemplateNames.ForgotPasswordTemplate);

            request.Template = template.Template;

            var data = new NotificationRequestDto
            {
                ToAddress = request.EmailAddress,
                FromAddress = this.emailSender,
                IgnoreTemplate = true,
                IsBodyHtml = true,
                Body = this.templateMapper.MapToForgotPasswordTemplateData(request),
                Subject = EmailSubjects.ForgotPassword
            };

            return await this.notificationService.AddNotificationAsync(data);
        }
    }
}
