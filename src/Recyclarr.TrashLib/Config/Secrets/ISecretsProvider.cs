using System.Collections.Immutable;

namespace Recyclarr.TrashLib;

public interface ISecretsProvider
{
    IImmutableDictionary<string, string> Secrets { get; }
}
