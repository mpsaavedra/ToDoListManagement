using Bootler.Contracts.Requests.Tasks;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Validations.Tasks;

public class GetAllTasksValidation : AbstractValidator<GetAllTasksRequest>
{
    public GetAllTasksValidation()
    {
        RuleFor(x => x.PageIndex)
            .Must(x => x > 0)
            .WithMessage("PageIndex must be greater then 0");
        RuleFor(x => x.PageSize)
            .Must(x => x < 1)
            .WithMessage("PageSize must be greater then 1");
    }
}
