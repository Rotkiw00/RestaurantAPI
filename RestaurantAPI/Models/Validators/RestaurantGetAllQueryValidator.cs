using FluentValidation;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantGetAllQueryValidator : AbstractValidator<RestaurantGetAllQuery>
    {
        private readonly int[] _allowedPageSizes = [5, 10, 15];
        private readonly string[] _allowedSortByColumnNames = [nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description)];

        public RestaurantGetAllQueryValidator()                
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!_allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in range [{string.Join(",", _allowedPageSizes)}]");
                }
            });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || _allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional or must be in range: [{string.Join(",", _allowedSortByColumnNames)}]");
        }
    }
}
