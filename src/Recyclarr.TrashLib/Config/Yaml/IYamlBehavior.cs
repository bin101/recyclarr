using YamlDotNet.Serialization;

namespace Recyclarr.TrashLib;

public interface IYamlBehavior
{
    void Setup(DeserializerBuilder builder);
}
