using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly ILogger<RestaurantService> _logger;

		public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_logger = logger;
		}

		public IEnumerable<RestaurantDto> GetAll()
		{
			var restaurants = _dbContext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.ToList();

			var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
			return restaurantsDto;
		}

		public RestaurantDto GetById(int id)
		{
			var restaurant = _dbContext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
			return restaurantDto;
		}

		public int Create(CreateRestaurantDto createRestaurantDto)
		{
			var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);

			_dbContext.Restaurants.Add(restaurant);
			_dbContext.SaveChanges();

			return restaurant.Id;
		}

		public void Update(UpdateRestaurantDto updateRestaurantDto, int id)
		{
			var restaurant = _dbContext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			restaurant.Name = updateRestaurantDto.Name;
			restaurant.Description = updateRestaurantDto.Description;
			restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

			_dbContext.SaveChanges();
		}

		public void Delete(int id)
		{
			var restaurant = _dbContext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) throw new NotFoundException("Restaurant not found");

			_dbContext.Restaurants.Remove(restaurant);
			_dbContext.SaveChanges();
		}		
	}
}
