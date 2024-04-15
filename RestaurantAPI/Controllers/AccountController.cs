using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _service;
        public AccountController(IAccountService service)
		{
			_service = service;
		}

		[HttpPost("register")]
		public ActionResult RegisterUser([FromBody] RegisterUserDto userDto)
		{
			_service.RegisterUser(userDto);
			return Ok();
		}

		[HttpPost("login")]
		public ActionResult LoginUser([FromBody]LoginUserDto userDto)
		{
			string tokenJwt = _service.GetJwtToken(userDto);
			return Ok(tokenJwt);
		}
	}
}
