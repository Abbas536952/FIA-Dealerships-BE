using DealershipManagement.Common.Utilities.Interfaces;
using DealershipManagement.Entities.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DealershipManagement.DataAccess
{
    public class WebWorkContext : IWorkContext<UserAccount, string>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DealershipManagementDbContext context;

        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            DealershipManagementDbContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public string LoggedInUserId => CurrentUser?.Id;

        public string Token
        {
            get
            {
                return GetToken();
            }
        }

        public UserAccount CurrentUser
        {
            get
            {
                var token = GetToken();

                var session = context.UserSessions
                    .Include(x => x.UserAccount)
                        .ThenInclude(x => x.Roles)
                            .ThenInclude(x => x.Role)
                    .Include(x => x.UserAccount)
                        .ThenInclude(x => x.Customer)
                    .FirstOrDefault(x => x.Token == token);

                return session?.UserAccount;
            }
        }

        public int EntityType => 0;

        public string EntityId => string.Empty;

        private string GetToken()
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var res = httpContextAccessor?.HttpContext.Request.Headers.TryGetValue("Authorization", out token) ?? false;
            if (res)
            {
                return token.ToString().Replace("Bearer ", string.Empty).Replace("bearer ", string.Empty);
            }
            return null;
        }
    }
}
