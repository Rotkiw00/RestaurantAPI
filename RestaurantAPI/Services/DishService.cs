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
			var restaurant = GetRestaurantFromDbById(restaurantId);

			var dishEntity = _mapper.Map<Dish>(createDishDto);

			dishEntity.RestaurantId = restaurantId;

			_dbContext.Dishes.Add(dishEntity);
			_dbContext.SaveChanges();

			return dishEntity.Id;
		}

		public List<DishDto> GetAll(int restaurantId)
		{
			var restaurant = GetRestaurantFromDbById(restaurantId);

			var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

			return dishDtos;
		}

		public DishDto GetById(int restaurantId, int dishId)
		{
			var restaurant = GetRestaurantFromDbById(restaurantId);

			var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);

			if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found");

			var dishDto = _mapper.Map<DishDto>(dish);

			return dishDto;
		}

		public void RemoveAll(int restaurantId)
		{
			var restaurant = GetRestaurantFromDbById(restaurantId);

			_dbContext.RemoveRange(restaurant.Dishes);
			_dbContext.SaveChanges();
		}

		private Restaurant GetRestaurantFromDbById(int restaurantId)
		{
			var restaurant = _dbContext
				.Restaurants
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == restaurantId) ?? throw new NotFoundException("Restaurant not found");

			return restaurant;
		}
	}
}
