﻿using Microsoft.AspNetCore.Mvc;
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
	}
}
