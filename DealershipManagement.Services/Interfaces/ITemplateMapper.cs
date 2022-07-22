using DealershipManagement.DataTransferObjects.Services.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface ITemplateMapper
    {
        string MapToAccountActivationTemplateData(AccountActivationNotificationRequestDto request);
        string MapToForgotPasswordTemplateData(ForgotPasswordNotificationRequestDto request);
    }
}
