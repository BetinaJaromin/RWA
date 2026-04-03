using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RWA.Api.Entities;

namespace RWA.Api.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value,context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if(emailInUse)
                    {
                        context.AddFailure("Email", "Email is already in use");
                    }
                });
        }
    }
}
