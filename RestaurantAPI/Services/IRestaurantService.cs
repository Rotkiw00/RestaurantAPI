using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto restaurantDto);
		void Update(UpdateRestaurantDto updateRestaurantDto, int id);
		void Delete(int id);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
	}
}