using YamlDotNet.Serialization;

namespace Recyclarr.TrashLib;

public interface IYamlSerializerFactory
{
    IDeserializer CreateDeserializer();
}
