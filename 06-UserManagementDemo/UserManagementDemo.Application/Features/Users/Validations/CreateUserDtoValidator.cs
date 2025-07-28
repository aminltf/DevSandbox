using FluentValidation;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Validations;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MinimumLength(3).WithMessage("UserName must be at least 3 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100);

        RuleFor(x => x.Role)
            .Must(value => Enum.IsDefined(typeof(UserRole), value))
            .WithMessage("Role is invalid.");
    }
}
