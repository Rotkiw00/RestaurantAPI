using FluentValidation;

namespace RestaurantAPI.Models.Validators
{
	public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
	{
		public RegisterUserDtoValidator(RestaurantDbContext dbContext)
		{
			RuleFor(dto => dto.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(dto => dto.Email)
				.Custom((value, context) =>
				{
					bool isEmailUsed = dbContext.Users.Any(u => u.Email == value);
					if (isEmailUsed)
					{ context.AddFailure("Email", $"{value} email is already taken"); }
				});

			RuleFor(dto => dto.Password)
				.NotEmpty()
				.MinimumLength(6);

			RuleFor(dto => dto.ConfirmPassword)
				.Equal(dto => dto.Password);
		}
	}
}
