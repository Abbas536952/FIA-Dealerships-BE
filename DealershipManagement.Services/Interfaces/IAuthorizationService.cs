using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> AuthorizeUserAsync(string token);
    }
}
