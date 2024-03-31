using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class DishService : IDishService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;

		public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
			_dbContext = dbContext;
			_mapper = mapper;
		}

        public int Create(int restaurantId, CreateDishDto createDishDto)
		{
			var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			var dishEntity = _mapper.Map<Dish>(createDishDto);

			dishEntity.RestaurantId = restaurantId;

			_dbContext.Dishes.Add(dishEntity);
			_dbContext.SaveChanges();

			return dishEntity.Id;
		}

		public List<DishDto> GetAll(int restaurantId)
		{
			var restaurant = _dbContext
				.Restaurants
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

			return dishDtos;
		}

		public DishDto GetById(int restaurantId, int dishId)
		{
			var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);

			if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found");

			var dishDto = _mapper.Map<DishDto>(dish);

			return dishDto;
		}
	}
}
