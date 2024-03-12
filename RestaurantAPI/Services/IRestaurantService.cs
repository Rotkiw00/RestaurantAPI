using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto restaurantDto);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
	}
}