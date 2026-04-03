using Microsoft.EntityFrameworkCore;
using RWA.Api.Entities;

namespace RWA.Api
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbcontext;
        public RestaurantSeeder(RestaurantDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Seed()
        {
            if ((_dbcontext.Database.CanConnect()))
            {
                var pendingMigrations = _dbcontext.Database.GetPendingMigrations();

                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbcontext.Database.Migrate();
                }

                if (!_dbcontext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbcontext.Roles.AddRange(roles);
                    _dbcontext.SaveChanges();
                }

                if (!_dbcontext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbcontext.Restaurants.AddRange(restaurants);
                    _dbcontext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User",
                },
                new Role()
                {
                    Name = "Manager",
                },
                new Role()
                {
                    Name = "Admin",
                }
            };

            return roles;
        }
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    Category = "Fast Food",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "nashville Hot Chicken",
                            Price = 10.38M
                        },
                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 5.38M
                        }
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald Szewska",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Szewska 2",
                        PostalCode = "30-001"
                    }
                }

            };

            return restaurants;
        }
    }
}
