using Bootler.Contracts.Requests.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Validations.Users;

public class SignUpValidation : AbstractValidator<SignUpRequest>
{
    public SignUpValidation()
    {
        RuleFor(x => x.UserName)
            .Must(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x))
            .WithMessage("UserName could not be null, empty or whitespace")
            .Must(x => x.Length < 150)
            .WithMessage("Username could not have more then 150 characters");
        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x))
            .WithMessage("Password could not be null, empty or whitespace");
        RuleFor(x => x.Role)
            .Must(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x))
            .WithMessage("Role could not be null, empty or whitespace");

    }
}
