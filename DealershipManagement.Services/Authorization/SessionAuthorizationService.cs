using DealershipManagement.DataAccess;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.Entities.Account;
using DealershipManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Template.Repository;

namespace DealershipManagement.Services.Authorization
{
    public class SessionAuthorizationService : IAuthorizationService
    {
        private readonly RepositoryBase<UserSession, DealershipManagementDbContext> userSessionRepository;
        private readonly IConfiguration configuration;

        public SessionAuthorizationService(
            RepositoryBase<UserSession, DealershipManagementDbContext> userSessionRepository,
            IConfiguration configuration)
        {
            this.userSessionRepository = userSessionRepository;
            this.configuration = configuration;
        }

        public async Task<bool> AuthorizeUserAsync(string token)
        {
            var sessionInDb = await userSessionRepository.GetAll().FirstOrDefaultAsync(x => x.Token == token);
            if (sessionInDb == null || sessionInDb.ExpiryDate <= DateTime.UtcNow)
                return false;

            var slidingExpirationInHours = Convert.ToInt32(configuration[AuthenticationConstants.SlidingExpirationTimeInHours]);
            sessionInDb.ExpiryDate = DateTime.UtcNow.AddHours(slidingExpirationInHours);

            await userSessionRepository.UpdateAsync(sessionInDb);
            await userSessionRepository.SaveAsync();

            return true;
        }
    }
}
