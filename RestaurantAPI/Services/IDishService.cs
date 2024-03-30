using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IDishService
	{
		int Create(int restaurantId, CreateDishDto createDishDto);
	}
}