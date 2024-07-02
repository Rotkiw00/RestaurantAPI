﻿using System.Security.Claims;

namespace RestaurantAPI.Services
{
	public class UserContextService : IUserContextService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserContextService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
		public int? UserId => int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
	}
}
