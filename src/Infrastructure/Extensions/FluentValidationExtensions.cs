using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Bootler;

public static class FluentValidationExtensions
{
    public static void IsInvalidThrow(this ValidationResult? source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (source.IsValid) 
            return;

        var errors = source.Errors?.Select(e => e.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m)) ?? Enumerable.Empty<string>();
        var message = "Validation errors: " + string.Join("; ", errors);

        throw new ValidationException(message);
    }
}
