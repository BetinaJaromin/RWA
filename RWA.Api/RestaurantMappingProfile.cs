using Mapster;
using RWA.Api.Entities;

namespace RWA.Api
{
    public class RestaurantMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entities.Restaurant, Models.RestaurantDto>()
                .Map(dest => dest.City, src => src.Address == null ? null : src.Address.City)
                .Map(dest => dest.Street, src => src.Address == null ? null : src.Address.Street)
                .Map(dest => dest.PostalCode, src => src.Address == null ? null : src.Address.PostalCode);

            config.NewConfig<Entities.Dish, Models.DishDto>();

            config.NewConfig<Models.CreateRestaurantDto, Entities.Restaurant>()
                .Map(dest => dest.Address, src => new Address()
                    {
                        City = src.City,
                        Street = src.Street,
                        PostalCode = src.PostalCode
                    });

            config.NewConfig<Models.CreateDishDto, Entities.Dish>();

            config.NewConfig<Models.CreateDishDto, Entities.Dish>();
        }
    }
}
