using Microsoft.AspNetCore.Authorization;

namespace RWA.Api.Authorization
{
    public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
    {
        public CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }

        public int MinimumRestaurantsCreated { get; }
    }
}
