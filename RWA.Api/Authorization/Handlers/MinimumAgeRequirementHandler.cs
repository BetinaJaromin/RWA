using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RWA.Api.Authorization.Handlers
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
            _logger.LogInformation($"User: {userEmail} is being evaluated for minimum age requirement of {requirement.MinimumAge} years.");

            DateTime.TryParse(context.User.FindFirst(c => c.Type == "DateOfBirth")?.Value, out DateTime dateOfBirth);
            if(dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Now)
            {
                _logger.LogInformation($"Authorization succeded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation($"Authorization failed");
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
