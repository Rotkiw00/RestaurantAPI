﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;

		public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
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

			if (restaurant is null) return null;

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
	}
}
