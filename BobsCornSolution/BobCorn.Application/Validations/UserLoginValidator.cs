using BobsCorn.Application.DTOs;
using FluentValidation;

namespace BobsCorn.Application.Validations
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
                .MaximumLength(20).WithMessage("Email cannot exceed 20 characters.");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(12).WithMessage("Password cannot exceed 12 characters.");
                //.Must(p => !p.Any(char.IsWhiteSpace)).WithMessage("Password cannot contain whitespace characters.");
        }
    }
}
