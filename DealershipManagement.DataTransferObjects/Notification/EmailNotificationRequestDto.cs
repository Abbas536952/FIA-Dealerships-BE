using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Notification
{
    public class EmailNotificationRequestDto
    {
        public int Code { get; set; }
        public string Body { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public string SendError { get; set; }
        public bool IgnoreTemplate { get; set; }
        public string TokenData { get; set; }
        [NotMapped]
        public Dictionary<string, object> Data { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public string ToAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        [NotMapped]
        public string[] ToAddresses { get; set; } = new string[0];
        public bool IsBodyHtml { get; set; }
        public string MessageId { get; set; }
    }
}
