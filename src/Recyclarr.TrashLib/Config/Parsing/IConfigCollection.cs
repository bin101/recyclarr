using Recyclarr.TrashLib.Config.Services;

namespace Recyclarr.TrashLib.Config.Parsing;

public interface IConfigCollection
{
    IReadOnlyCollection<IServiceConfiguration> Get(SupportedServices? serviceType);
    bool DoesConfigExist(string name);
}
