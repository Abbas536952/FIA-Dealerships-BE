using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.Common.Utilities.Interfaces
{
    public interface IWorkContext<TAccount, TKey>
    {
        TKey LoggedInUserId { get; }

        string Token { get; }

        TAccount CurrentUser { get; }

        int EntityType { get; }

        string EntityId { get; }
    }
}
