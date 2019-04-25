using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Authorization
{
    public class AdminAuthHandler : AuthorizationHandler<AdminUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminUserRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
