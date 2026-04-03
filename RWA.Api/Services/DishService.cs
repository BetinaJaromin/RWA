using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RWA.Api.Entities;
using RWA.Api.Exceptions;
using RWA.Api.Models;
using RWA.Api.Services.Interfaces;

namespace RWA.Api.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);

            dish.RestaurantId = restaurantId;

            _context.Dishes.Add(dish);
            _context.SaveChanges();

            return dish.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = GetDishById(restaurantId,dishId);

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishDtos = _mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes!);
            return dishDtos;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            _context.RemoveRange(restaurant.Dishes!);
            _context.SaveChanges();
        }

        public void RemoveById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = GetDishById(restaurantId, dishId);

            _context.Remove(dish);
            _context.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _context.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            return restaurant;
        }

        private Dish GetDishById(int restaurantId, int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");
            return dish;
        }
    }
}
