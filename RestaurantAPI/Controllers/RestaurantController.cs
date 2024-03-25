using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	[ApiController]
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
		public ActionResult<Restaurant> GetById([FromRoute]int id)
		{
			var restaurantDto = _restaurantService.GetById(id);

			return Ok(restaurantDto);
		}

		[HttpPost]
		public ActionResult Create([FromBody]CreateRestaurantDto restaurantDto)
		{
			int id = _restaurantService.Create(restaurantDto);

			return Created($"api/restaurant/{id}", null);
		}

		[HttpPut("{id}")]
		public ActionResult UpdateById([FromBody]UpdateRestaurantDto updateRestaurantDto, [FromRoute]int id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			_restaurantService.Update(updateRestaurantDto, id);

			return Ok();
		}

		[HttpDelete("{id}")]
		public ActionResult DeleteById([FromRoute]int id)
		{
			_restaurantService.Delete(id);

			return NoContent();
        }
    }
}
