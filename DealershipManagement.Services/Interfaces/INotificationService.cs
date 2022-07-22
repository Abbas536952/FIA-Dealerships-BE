using DealershipManagement.DataTransferObjects.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface INotificationService
    {
        Task<string> AddNotificationAsync(NotificationRequestDto notificationDto);
    }
}
