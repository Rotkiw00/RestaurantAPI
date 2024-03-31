using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IDishService
	{
		int Create(int restaurantId, CreateDishDto createDishDto);
		DishDto GetById(int restaurantId, int dishId);
		List<DishDto> GetAll(int restaurantId);
	}
}