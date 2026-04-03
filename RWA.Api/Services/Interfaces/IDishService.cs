using RWA.Api.Models;

namespace RWA.Api.Services.Interfaces
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById (int restaurantId, int dishId);
        IEnumerable<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
        void RemoveById(int restaurantId, int dishId);
    }
}
