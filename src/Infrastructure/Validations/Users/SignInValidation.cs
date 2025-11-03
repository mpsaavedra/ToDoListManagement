using Bootler.Contracts.Requests.Users;
using Bootler.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Validations.Users;

public class SignInValidation : AbstractValidator<SignInRequest>
{
    public SignInValidation()
    {
        RuleFor(x => x.UserName)
                    .Must(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x))
                    .WithMessage("UserName could not be null, empty or whitespace")
                    .Must(x => x.Length < 150)
                    .WithMessage("Username could not have more then 150 characters");
        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x))
            .WithMessage("Password could not be null, empty or whitespace");
    }
}
