using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace Recyclarr.TrashLib;

[UsedImplicitly]
public class SecretsYamlBehavior : IYamlBehavior
{
    private readonly ISecretsProvider _secretsProvider;

    public SecretsYamlBehavior(ISecretsProvider secretsProvider)
    {
        _secretsProvider = secretsProvider;
    }

    public void Setup(DeserializerBuilder builder)
    {
        builder
            .WithNodeDeserializer(new SecretsDeserializer(_secretsProvider))
            .WithTagMapping("!secret", typeof(SecretTag));
    }
}
