using BobsCorn.Application.DTOs;
using FluentValidation;

namespace BobsCorn.Application.Validations
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
                .MaximumLength(20).WithMessage("Email cannot exceed 20 characters.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(12).WithMessage("Password cannot exceed 12 characters.")
                .Must(p => !p.Any(char.IsWhiteSpace)).WithMessage("Password cannot contain whitespace characters.");

            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .Must(name => name.All(char.IsLetter)).WithMessage("Name cannot contain special characters or digits.");

            RuleFor(u => u.Lastname)
                .NotEmpty().WithMessage("Lastname cannot be empty.")
                .MinimumLength(2).WithMessage("Lastname must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Lastname cannot exceed 50 characters.")
                .Must(lastname => lastname.All(char.IsLetter)).WithMessage("Lastname cannot contain special characters or digits.");
        }
    }
}
