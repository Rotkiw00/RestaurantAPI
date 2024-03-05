using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using AutoMapper;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	public class RestaurantController : ControllerBase
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;

		public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Restaurant>> GetAll()
		{
			var restaurants = _dbContext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.ToList();

			/* Wrong way for mapping object to its DTO
			 better to use AutoMapper

			var restaurantsDto = restaurants.Select(r => new RestaurantDto()
			{
				Name = r.Name,
				Description = r.Description,
				Category = r.Category,
				HasDelivery = r.HasDelivery,
			});
			
			 */

			var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
			return Ok(restaurantsDto);
		}

		[HttpGet("{id}")]
		public ActionResult<Restaurant> Get([FromRoute]int id)
		{
			var restaurant = _dbContext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) return NotFound();

			var restaurantDto = _mapper.Map<RestaurantDto>(restaurant); // <T> is the generic type which is the destination type to return
			return Ok(restaurantDto);
		}
    }
}
