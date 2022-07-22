using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Services.Notification;
using DealershipManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Mapper
{
    public class TemplateMapper : ITemplateMapper
    {
        private readonly IConfiguration configuration;

        public TemplateMapper(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string MapToAccountActivationTemplateData(AccountActivationNotificationRequestDto request)
        {
            var activationUrlTokens = new Dictionary<string, string>();
            activationUrlTokens.Add(EmailTemplateTokens.UserId, request.UserId);

            var setPasswordLink = this.configuration["SetPasswordLink"];
            setPasswordLink = this.ReplaceTokens(setPasswordLink, activationUrlTokens);

            var dataTokens = new Dictionary<string, string>();
            dataTokens.Add(EmailTemplateTokens.UserFirstName, request.FirstName);
            dataTokens.Add(EmailTemplateTokens.UserId, request.UserId);
            dataTokens.Add(EmailTemplateTokens.SetPasswordLink, setPasswordLink);

            var template = this.MapToTemplate(dataTokens, request.Template);

            return template;
        }

        public string MapToForgotPasswordTemplateData(ForgotPasswordNotificationRequestDto request)
        {
            var resetPasswordTokens = new Dictionary<string, string>();
            resetPasswordTokens.Add(EmailTemplateTokens.UserId, request.UserId);
            resetPasswordTokens.Add(EmailTemplateTokens.Token, request.Token);
            resetPasswordTokens.Add(EmailTemplateTokens.UserName, request.UserName);

            var resetPasswordLink = this.configuration["ResetPasswordLink"];
            resetPasswordLink = this.ReplaceTokens(resetPasswordLink, resetPasswordTokens);

            var dataTokens = new Dictionary<string, string>();
            dataTokens.Add(EmailTemplateTokens.UserFirstName, request.FirstName);
            dataTokens.Add(EmailTemplateTokens.Token, request.Token);
            dataTokens.Add(EmailTemplateTokens.ResetPasswordLink, resetPasswordLink);

            var template = this.MapToTemplate(dataTokens, request.Template);

            return template;
        }

        private string MapToTemplate(Dictionary<string, string> dataTokens, string template)
        {
            template = this.ReplaceTokens(template, dataTokens);
            return template;
        }

        public string ReplaceTokens(string template, Dictionary<string, object> data)
            => this.ReplaceTokens(template, data.ToDictionary(x => x.Key, x => $"{x.Value}"));

        public string ReplaceTokens(string template, Dictionary<string, string> data)
        {
            foreach (var token in data)
            {
                template = template.Replace($"[{token.Key}]", $"{token.Value}", StringComparison.InvariantCultureIgnoreCase);
            }

            return template;
        }
    }
}
