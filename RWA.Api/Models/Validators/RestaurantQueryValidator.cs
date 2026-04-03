using FluentValidation;
using RWA.Api.Entities;

namespace RWA.Api.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSize = new[] { 5, 10, 15 };
        private string[] allowedSortBy = new[] { 
            nameof(Restaurant.Name),nameof(Restaurant.Category),nameof(Restaurant.Description) };
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if(!allowedPageSize.Contains(value))
                {
                    context.AddFailure("PageSize", $"Page size must be in [{string.Join(", ", allowedPageSize)}]");
                }
            });
            RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortBy.Contains(value))
                .WithMessage($"SortBy must be empty or one of the following values: {string.Join(", ", allowedSortBy)}");
        }
    }
}
