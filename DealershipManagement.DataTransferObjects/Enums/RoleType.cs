using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Enums
{
    public enum RoleType
    {
        Invalid = 0,
        [Description("Global Admin")]
        GlobalAdmin = 1,
        [Description("Customer")]
        Customer = 2
    }
}
