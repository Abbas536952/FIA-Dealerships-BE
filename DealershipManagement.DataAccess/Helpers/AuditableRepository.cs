using DealershipManagement.Common.Utilities.Interfaces;
using DealershipManagement.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template.Repository;

namespace DealershipManagement.DataAccess.Helpers
{
    public class AuditableRepository<T> : RepositoryBase<T, DealershipManagementDbContext>
        where T : class
    {
        private readonly IWorkContext<UserAccount, string> workContext;
        private readonly DealershipManagementDbContext context;

        public AuditableRepository(DealershipManagementDbContext context, IWorkContext<UserAccount, string> workContext, bool enableSoftDelete = false)
            : base(context, enableSoftDelete)
        {
            this.workContext = workContext;
            this.context = context;
        }

        public override async Task SaveAsync(CancellationToken token = default)
        {
            ProcessEntities(workContext?.LoggedInUserId);

            await this.context.SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}
