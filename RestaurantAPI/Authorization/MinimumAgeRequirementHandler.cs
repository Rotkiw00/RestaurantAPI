using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
	public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
		{
			var s = context.User.FindFirst(c => c.Type == "DateOfBirth");

			var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
		}
	}
}
