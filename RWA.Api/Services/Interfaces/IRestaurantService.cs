using RWA.Api.Models;
using System.Security.Claims;

namespace RWA.Api.Services.Interfaces
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        int Create(CreateRestaurantDto dto);

        void Delete(int id);

        void Update(int id, UpdateRestaurantDto dto);
    }
}
