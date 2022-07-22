using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Notification
{
    public class NotificationRequestDto
    {
        public string Body { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool IgnoreTemplate { get; set; }
        public string TemplateName { get; set; }
        public int Code { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public string Notes { get; set; }
        public string ToUserId { get; set; }
        public override string ToString() => $"To:{ToAddress} From:{FromAddress}, Body:{Body}";
    }
}
