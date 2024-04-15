using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
	public class AccountService : IAccountService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IPasswordHasher<User> _passwordHasher;
		private readonly AuthenticationSettings _authSettings;


		public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authSettings)
        {
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
			_authSettings = authSettings;
		}

		public string GetJwtToken(LoginUserDto loginUserDto)
		{
			var user = _dbContext.Users
				.Include(u => u.Role)
				.FirstOrDefault(u => u.Email == loginUserDto.Email) 
				?? throw new BadRequestException("Invalid username or password");

			var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);

			if (passwordVerification == PasswordVerificationResult.Failed)
			{ throw new BadRequestException("Invalid username or password"); }

			var claims = new List<Claim>()
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
				new(ClaimTypes.Role, $"{user.Role.Name}"),
				new("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
				new("Nationality", user.Nationality)
			};

			SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
			SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
			DateTime expires = DateTime.Now.AddDays(_authSettings.JwtExpireDays);

			JwtSecurityToken token = new(_authSettings.JwtIssuer,
				_authSettings.JwtIssuer,
				claims,
				expires: expires,
				signingCredentials: credentials);

			JwtSecurityTokenHandler tokenHandler = new();
			return tokenHandler.WriteToken(token);
		}

		public void RegisterUser(RegisterUserDto userDto)
		{
			var user = new User()
			{
				Email = userDto.Email,
				DateOfBirth = userDto.DateOfBirth,
				Nationality = userDto.Nationality,
				RoleId = userDto.RoleId,
			};

			var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);
			user.PasswordHash = hashedPassword;

			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
		}
	}
}
