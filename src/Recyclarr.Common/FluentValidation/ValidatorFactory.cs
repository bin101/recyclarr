using Autofac;
using FluentValidation;

namespace Recyclarr.Common;

public class ValidatorFactory : IValidatorFactory
{
    private readonly ILifetimeScope _scope;

    public ValidatorFactory(ILifetimeScope scope)
    {
        _scope = scope;
    }

    public IValidator GetValidator(Type typeToValidate)
    {
        return (IValidator) _scope.ResolveGeneric(typeof(IValidator<>), typeToValidate);
    }
}
