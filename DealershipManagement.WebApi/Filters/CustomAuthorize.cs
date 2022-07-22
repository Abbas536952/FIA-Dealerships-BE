using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Repository;
using System.Net;
using DealershipManagement.Common;
using DealershipManagement.Common.Helpers;
using DealershipManagement.Entities.Account;
using DealershipManagement.DataAccess;
using DealershipManagement.Services.Interfaces;
using DealershipManagement.Services.IdentityManager;

namespace DealershipManagement.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorize : ActionFilterAttribute
    {
        private readonly string[] roles;
        public string Claim;

        public CustomAuthorize(params string[] roles) => this.roles = roles;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!this.SkipAllowAnonymous(context))
            {
                var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorizationHeader))
                    CustomErrors.Unauthorized.ThrowCustomErrorException(HttpStatusCode.Unauthorized);

                var token = authorizationHeader.ToString().Split(' ').Last();

                var userSessionRepository = (RepositoryBase<UserSession, DealershipManagementDbContext>)context.HttpContext.RequestServices.GetService(
                    typeof(RepositoryBase<UserSession, DealershipManagementDbContext>));

                var session = userSessionRepository.GetAll()
                        .Include(x => x.UserAccount)
                            .ThenInclude(x => x.Roles)
                                .ThenInclude(x => x.Role)
                        .FirstOrDefaultAsync(x => x.Token == token).Result;

                var userManager = (IUserManagerAccessor)context.HttpContext.RequestServices.GetService(typeof(UserManagerAccessor));

                if (session == null || session.ExpiryDate <= DateTime.UtcNow ||
                    (roles.Any() && !await userManager.IsInAnyRoleAsync(session.UserAccount, this.roles.ToArray())))
                    CustomErrors.Unauthorized.ThrowCustomErrorException(HttpStatusCode.Unauthorized);

                if (session.RememberMe == null  || !session.RememberMe.Value)
                    session.ExpiryDate = DateTime.UtcNow.AddDays(1);

                await userSessionRepository.UpdateAsync(session);
                await userSessionRepository.SaveAsync();

                if (!string.IsNullOrEmpty(this.Claim))
                {  
                    var roleClaimRepo = (RepositoryBase<RoleClaim, DealershipManagementDbContext>)context.HttpContext.RequestServices.GetService(
                        typeof(RepositoryBase<RoleClaim, DealershipManagementDbContext>));

                    var userRoles = session.UserAccount.Roles.Select(x => x.RoleId);
                    var claimExist = roleClaimRepo.GetAll().Any(x => x.Claim.Name == this.Claim && userRoles.Contains(x.RoleId));

                    if (!claimExist)
                        CustomErrors.Unauthorized.ThrowCustomErrorException(HttpStatusCode.Unauthorized);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private bool SkipAllowAnonymous(ActionExecutingContext filterContext)
        {
            var controllerDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

            return filterContext.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
