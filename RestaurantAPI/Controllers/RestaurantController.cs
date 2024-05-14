using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	[ApiController]
	[Authorize]
	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		public RestaurantController(IRestaurantService restaurantService)
        {
			_restaurantService = restaurantService;
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult<IEnumerable<Restaurant>> GetAll()
		{
			var restaurantsDto = _restaurantService.GetAll();
			return Ok(restaurantsDto);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public ActionResult<Restaurant> GetById([FromRoute]int id)
		{
			var restaurantDto = _restaurantService.GetById(id);

			return Ok(restaurantDto);
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Manager")]
		public ActionResult Create([FromBody]CreateRestaurantDto restaurantDto)
		{
			int userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

			int id = _restaurantService.Create(restaurantDto);

			return Created($"api/restaurant/{id}", null);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin,Manager")]
		public ActionResult UpdateById([FromBody] UpdateRestaurantDto updateRestaurantDto, [FromRoute]int id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			_restaurantService.Update(updateRestaurantDto, id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin,Manager")]
		public ActionResult DeleteById([FromRoute]int id)
		{
			_restaurantService.Delete(id);

			return NoContent();
        }
    }
}
