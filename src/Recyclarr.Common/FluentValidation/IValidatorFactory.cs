using FluentValidation;

namespace Recyclarr.Common;

public interface IValidatorFactory
{
    IValidator GetValidator(Type typeToValidate);
}
