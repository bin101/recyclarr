using Recyclarr.Common;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Recyclarr.TrashLib;

public record EnvironmentVariableTag;

public class EnvironmentVariablesDeserializer : INodeDeserializer
{
    private readonly IEnvironment _environment;

    public EnvironmentVariablesDeserializer(IEnvironment environment)
    {
        _environment = environment;
    }

    public bool Deserialize(
        IParser reader,
        Type expectedType,
        Func<IParser, Type, object?> nestedObjectDeserializer,
        out object? value)
    {
        // Only process items flagged as Environment Variables
        if (expectedType != typeof(EnvironmentVariableTag))
        {
            value = null;
            return false;
        }

        var scalar = reader.Consume<Scalar>();
        var split = scalar.Value.Trim().Split(' ', 2);
        var envVarValue = _environment.GetEnvironmentVariable(split[0]);
        if (string.IsNullOrWhiteSpace(envVarValue))
        {
            // Trim whitespace + quotation characters
            envVarValue = split.ElementAtOrDefault(1)?.Trim().Trim('\'', '"');
        }

        value = envVarValue ?? throw new EnvironmentVariableNotDefinedException(scalar.Start.Line, scalar.Value);
        return true;
    }
}
