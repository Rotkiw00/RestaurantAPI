namespace RestaurantAPI
{
	public class RestaurantSeeder
	{
		private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext context)
        {
            _dbContext = context;
        }

        public void Seed()
		{
			if (_dbContext.Database.CanConnect())
			{
				if (!_dbContext.Roles.Any())
				{
					var roles = GetRoles();
					_dbContext.Roles.AddRange(roles);
					_dbContext.SaveChanges();
				}

				if (!_dbContext.Restaurants.Any())
				{
					var restaurants = GetRestaurants();
					_dbContext.Restaurants.AddRange(restaurants);
					_dbContext.SaveChanges();
				}
			}
		}

		private IEnumerable<Role> GetRoles()
		{
			var roles = new List<Role>()
			{
				new()
				{
					Name = "User",
				},
				new()
				{
					Name = "Manager",
				},
				new()
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
				new()
				{
					Name = "KFC",
					Category = "Fast Food",
					Description = "KFC is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken",
					ContactEmail = "contact@kfc.com",
					HasDelivery = true,
					Dishes =
					[
						new Dish()
						{
							Name = "Nashville Hot Chicken",
							Price = 10.30M
						},
						new Dish()
						{
							Name = "Longer with strips",
							Price = 5.50M
						}
					],
					Address = new Address()
					{
						City = "Katowice",
						Street = "Mariacka",
						PostalCode = "40-100"
					}
				},
				new()
				{
					Name = "McDonald",
					Category = "Fast Food",
					Description = "McDonald's Corporation is an American multinational fast food chain, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United State",
					ContactEmail = "contact@mc.com",
					HasDelivery = true,
					Dishes =
					[
						new Dish()
						{
							Name = "Chicken Mc Nuggets",
							Price = 6.30M
						},
						new Dish()
						{
							Name = "BigMac",
							Price = 7.50M
						}
					],
					Address = new Address()
					{
						City = "Kraków",
						Street = "Baildona",
						PostalCode = "40-100"
					}
				}
			};

			return restaurants;
		}
	}
}
