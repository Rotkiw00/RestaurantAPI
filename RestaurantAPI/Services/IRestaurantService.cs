using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto restaurantDto);
		bool Update(UpdateRestaurantDto updateRestaurantDto, int id);
		bool Delete(int id);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
	}
}