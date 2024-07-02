using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Linq.Expressions;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext,
                                 IMapper mapper,
                                 ILogger<RestaurantService> logger,
                                 IAuthorizationService authorizationService,
                                 IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public PaginatedResult<RestaurantDto> GetAll(RestaurantGetAllQuery query)
        {
            var baseQuery = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                           r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                Dictionary<string, Expression<Func<Restaurant, object>>> columnSelectors = new()
                {
                    {nameof(Restaurant.Name), r => r.Name},
                    {nameof(Restaurant.Category), r => r.Category},
                    {nameof(Restaurant.Description), r => r.Description}
                };

                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortOrderResult == SortOrderResult.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            List<Restaurant> restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            int totalItemsCount = baseQuery.Count();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            var paginatedResults = new PaginatedResult<RestaurantDto>(restaurantsDto, totalItemsCount, query.PageSize, query.PageNumber);

            return paginatedResults;
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

            restaurant.CreatedById = _userContextService.UserId;

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

            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authResult.Succeeded)
            {
                throw new ForbiddenException();
            }

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

            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }
    }
}
