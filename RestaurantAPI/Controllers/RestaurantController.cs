using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		public RestaurantController(IRestaurantService restaurantService)
        {
			_restaurantService = restaurantService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Restaurant>> GetAll()
		{
			var restaurantsDto = _restaurantService.GetAll();
			return Ok(restaurantsDto);
		}

		[HttpGet("{id}")]
		public ActionResult<Restaurant> Get([FromRoute]int id)
		{
			var restaurantDto = _restaurantService.GetById(id);

			if (restaurantDto is null) return NotFound();

			return Ok(restaurantDto);
		}

		[HttpPost]
		public ActionResult CreateRestaurant([FromBody]CreateRestaurantDto restaurantDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			int id = _restaurantService.Create(restaurantDto);

			return Created($"api/restaurant/{id}", null);
		}
    }
}
