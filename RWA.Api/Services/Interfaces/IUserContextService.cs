using System.Security.Claims;

namespace RWA.Api.Services.Interfaces
{
    public interface IUserContextService
    {
         ClaimsPrincipal User { get; }
         int? GetUserId { get; }
    }

}
