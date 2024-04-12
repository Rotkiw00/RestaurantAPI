using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant/{restaurantId}/dish")]
	[ApiController]
	public class DishController : ControllerBase
	{
		private readonly IDishService _dishService;

		public DishController(IDishService dishService)
        {
			_dishService = dishService;
		}

		[HttpPost]
        public ActionResult Create([FromRoute]int restaurantId, [FromBody]CreateDishDto dishDto)
		{
			int dishId = _dishService.Create(restaurantId, dishDto);

			return Created($"api/restaurant/{restaurantId}/dish/{dishId}", null);
		}

		[HttpGet("{dishId}")]
		public ActionResult<DishDto> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
		{
			var dishDto = _dishService.GetById(restaurantId, dishId);

			return Ok(dishDto);
		}

		[HttpGet]
		public ActionResult<List<DishDto>> GetAll([FromRoute]int restaurantId)
		{
			var dishDtos = _dishService.GetAll(restaurantId);
			
			return Ok(dishDtos);
		}

		[HttpDelete]
		public ActionResult RemoveAll([FromRoute]int restaurantId)
		{
			_dishService.RemoveAll(restaurantId);

			return NoContent();
		}
		//TODO: Implement another DELETE method but for single entity removal
	}
}
