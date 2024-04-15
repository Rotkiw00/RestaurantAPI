using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
	{
		void RegisterUser(RegisterUserDto userDto);
		string GetJwtToken(LoginUserDto loginUserDto);
	}
}
