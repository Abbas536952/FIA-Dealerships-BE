using DealershipManagement.DataTransferObjects.Services.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface INotificationSenderService
    {
        Task<string> SendAccountActivationEmailAsync(AccountActivationNotificationRequestDto request);
        Task<string> SendForgotPasswordEmailAsync(ForgotPasswordNotificationRequestDto request);
    }
}
