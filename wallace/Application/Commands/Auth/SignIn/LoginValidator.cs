using FluentValidation;

namespace Wallace.Application.Commands.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(s => s.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(s => s.Password)
                .MinimumLength(8)
                .NotEmpty();
        }
    }
}